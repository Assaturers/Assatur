using Magicka.Extensions;
using Magicka.Logging;
using System;
using System.Collections.Generic;
using System.IO;

namespace Magicka.ModLoader;

public partial class ModLoader
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
            var modTypes = assembly.DefinedTypes.ConcreteSubclasses<Mod>();

            if (modTypes.Count == 0)
                _logger.Warning(LoggerSource, $"Could not find any class of type {nameof(Mod)} in assembly {Path.GetFileName(assembly.Location)}");
            else
            {
                if (modTypes.Count > 1)
                    _logger.Critical(LoggerSource, new("The ModLoader does not yet support multiple mod classes per assembly."));
                else
                {
                    var modType = modTypes[0];

                    try
                    {
                        var mod = CreateMod(modType);
                        
                        mod.Logger = Logger.GetLogger(mod.GetType());
                        mod.Assembly = assembly;

                        mods.Add(mod);
                        mod.LoadContent();
                    }
                    catch (Exception e)
                    {
                        _logger.Error(LoggerSource, $"Error while initializing mod {modType.GetType().Name}.", e);
                    }
                }
            }
        }

        for (int i = 0; i < mods.Count; i++)
            mods[i].PostLoadContent();

        Mods = mods.AsReadOnly();

        InitializeFactories();
    }

    private Mod CreateMod(Type type)
    {
        _logger.Log(LoggerSource, $"Initializing {type.GetType().Name}.");
        return (Mod)Activator.CreateInstance(type, true);
    }

    public void Do(Action<Mod> action) => Mods.Do(action);

    // private void CreateGlobals<>

    public IReadOnlyList<Mod> Mods { get; private set; }
}