using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Magicka.ModLoader;

public partial class ModLoader
{
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
