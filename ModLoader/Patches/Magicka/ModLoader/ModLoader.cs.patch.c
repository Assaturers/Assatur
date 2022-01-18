using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Magicka.Logging;

namespace Magicka.ModLoader;

public class ModLoader
{
    private const Logger.Source LoggerSource = Logger.Source.ModLoader;

    private readonly Logger _logger;

    private readonly MLRootProvider _root;
    private readonly AssembliesProvider _assemblies;

    public ModLoader()
    {
        _logger = Logger.GetLogger<ModLoader>();

        _root = new();
        _assemblies = new(_root);
    }

    internal void LoadContent()
    {
        _logger.Log(LoggerSource, "Initializing ModLoader.");

        _logger.Log(LoggerSource, $"Searching for assemblies in {_root.Directory.FullName}.");
        var assemblies = _assemblies.FindAssemblies();

        _logger.Log(LoggerSource, $"Initializing {nameof(Mod)} classes.");
        List<Mod> mods = new();

        foreach (var assembly in assemblies)
        {
            var modTypes = GetModTypes(assembly);

            if (modTypes.Count == 0)
                _logger.Warning(LoggerSource, $"Could not find any class of type {nameof(Mod)} in assembly {Path.GetFileName(assembly.Location)}");
            else
            {
                if (modTypes.Count > 1)
                    _logger.Critical(LoggerSource, new("The ModLoader does not yet support multiple mod classes per assembly."));
                else
                {
                    var modType = modTypes[0];
                    var mod = CreateMod(modType);

                    mod.Logger = Logger.GetLogger(mod.GetType());

                    mods.Add(mod);
                    mod.LoadContent();
                }
            }
        }

        for (int i = 0; i < mods.Count; i++)
            mods[i].PostLoadContent();
    }

    private List<Type> GetModTypes(Assembly assembly)
    {
        List<Type> types = new();

        foreach (var type in assembly.DefinedTypes)
            if (type.IsSubclassOf(typeof(Mod)))
                types.Add(type);

        return types;
    }

    private Mod CreateMod(Type type)
    {
        _logger.Log(LoggerSource, $"Initializing {type.GetType().Name}.");
        return (Mod)Activator.CreateInstance(type, true);
    }

    public IReadOnlyList<Mod> Mods { get; private set; }

    public class AssembliesProvider
    {
        private const string ModsFolder = "Mods";

        public AssembliesProvider(MLRootProvider root)
        {
            Directory = new(Path.Combine(root.Directory.FullName, ModsFolder));
        }

        public List<Assembly> FindAssemblies()
        {
            Directory.Create();
            List<Assembly> assemblies = new();

            foreach (var file in Directory.EnumerateFiles("*.dll", SearchOption.AllDirectories))
            {
                try
                {
                    var assembly = GetAssembly(file);
                    assemblies.Add(assembly);
                }
                catch
                {
                    ;
                }
            }

            return assemblies;
        }

        public Assembly GetAssembly(FileInfo file)
        {
            return Assembly.LoadFile(file.FullName);
        }

        public DirectoryInfo Directory { get; }
    }

    public class MLRootProvider
    {
        private const string Linux = "HOME";
        private const string Windows = "USERPROFILE";
        private const string AssaturFolder = "Assatur";

        public MLRootProvider()
        {
            // TODO If we ever manage to make this work on Linux, definitively want to add support for it.

            Directory = System.IO.Directory.CreateDirectory(
                @$"{Environment.GetEnvironmentVariable(Windows)}\Documents\My Games\Magicka\{AssaturFolder}");
        }

        public DirectoryInfo Directory { get; }
    }
}