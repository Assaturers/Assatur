using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Build.Execution;
using ModdingToolkit.Magicka.Decompiling;
using ModdingToolkit.Magicka.Finding;
using ModdingToolkit.Patching;
using Webmilio.Commons.Console;
using Webmilio.Commons.DependencyInjection;
using Webmilio.Commons.Extensions;

namespace ModdingToolkit.Magicka.Installing
{
    [Service]
    public class AssaturInstaller : IAssaturInstaller
    {
        private readonly ILocationStore _finder;
        private readonly IPatcher _patcher;
        private readonly IDecompiler _decompiler;

        public AssaturInstaller(ILocationStore finder, IDecompiler decompiler, IPatcher patcher)
        {
            _finder = finder;
            _patcher = patcher;
            _decompiler = decompiler;
        }

        public async Task Install()
        {
            Console.WriteLine("Welcome to the Assatur Installer!");
            Console.WriteLine("This installer will guide you through the steps required to install Assatur, the Magicka ModLoader created by Webmilio.");

            Console.WriteLine("We'll begin with finding where you have Magicka installed.");

            var installLocation = _finder.MagickaExecutable.Directory;
            Console.WriteLine($"Magicka installed on {installLocation}...");

            Console.WriteLine();

            DirectoryInfo tempDir = new(Path.Combine(Path.GetTempPath(), "Magicka"));
            tempDir.Create();

            DirectoryInfo decompileDir = tempDir.CreateSubdirectory("Decompiled");


            Console.Write("Decompiling Magicka.exe into {0}, this can take a long time... ", decompileDir);
            await _decompiler.DecompileFile(installLocation.FullName, decompileDir.FullName);
            Console.WriteLine("Done.");

            Console.Write("Applying patches... ");
            // _patcher.Patch();
            Console.WriteLine("Done.");

            Console.Write("Validating build... ");

            ProjectInstance projectInstance = new(decompileDir.CombineString("Magicka.csproj"));
            bool built = false;

            try
            {
                built = projectInstance.Build();
                Console.WriteLine("Done.");
            }
            catch (InvalidOperationException)
            {
                ConsoleHelper.WriteLineError("Error while rebuilding! Your Magicka instance has not been affected.");
            }

            Cleanup(tempDir);
        }


        private static void Cleanup(DirectoryInfo dir)
        {
            Console.Write("Deleting temporary Magicka folder... ");
            dir.Delete(true);
            Console.WriteLine("Done.");
        }
    }
}