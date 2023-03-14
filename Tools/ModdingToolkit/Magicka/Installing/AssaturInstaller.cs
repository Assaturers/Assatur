using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ModdingToolkit.Building;
using ModdingToolkit.Core;
using ModdingToolkit.Magicka.Decompiling;
using ModdingToolkit.Magicka.Finding;
using ModdingToolkit.Patching;
using ModdingToolkit.Source;
using Webmilio.Commons.DependencyInjection;
using Webmilio.Commons.Extensions;

namespace ModdingToolkit.Magicka.Installing
{
    [Service]
    public class AssaturInstaller : IAssaturInstaller
    {
        private readonly ILocationStore _loc;
        private readonly IDecompiler _decompiler;
        private readonly ISourceProvider _source;
        private readonly IPatcher _patcher;
        private readonly IBuilder _builder;

        public AssaturInstaller(ILocationStore loc, IDecompiler decompiler, ISourceProvider source, IPatcher patcher, IBuilder builder)
        {
            _loc = loc;
            _decompiler = decompiler;
            _source = source;
            _patcher = patcher;
            _builder = builder;
        }

        public async Task Install()
        {
            Console.WriteLine("Welcome to the Assatur Installer!");
            Console.WriteLine("This installer will guide you through the steps required to install Assatur, the Magicka ModLoader created by Webmilio.");

            Console.WriteLine("We'll begin with finding where you have Magicka installed.");
            Console.WriteLine($"Magicka installed on {_loc.MagickaExe.Directory}...");

            Console.WriteLine();

            DirectoryInfo tempDir = new(Path.Combine(Path.GetTempPath(), "Magicka"));
            tempDir.Delete(true);
            tempDir.Create();

            DirectoryInfo decompileDir = tempDir.CreateSubdirectory("Decompiled");
            DirectoryInfo sourceDir = tempDir.CreateSubdirectory("Source");

            Console.Write("Decompiling {0} into {1}, this can take a long time... ", Constants.MagickaExe, decompileDir);
            _loc.RestoreBackup();
            await _decompiler.DecompileFile(_loc.MagickaExe.FullName, decompileDir.FullName);
            Console.WriteLine("Done.");

            Console.Write("Acquiring remote source...");
            await _source.DownloadSource(sourceDir);
            Console.WriteLine("Done.");

            Console.Write("Applying patches... ");
            _patcher.Patch(sourceDir.GetDirectories()[0].Combine(@"\ModLoader", Constants.PatchesFolder), decompileDir);
            Console.WriteLine("Done.");

            Console.Write("Validating build... ");
            await _builder.Build(decompileDir.CombineString("Magicka.csproj"));
            Console.WriteLine("Magicka successfully rebuilt.");

            string[] toCopy = { Constants.MagickaExe, Constants.MagickaConfig  };

            var from = Path.Combine(decompileDir.FullName, "bin", Constants.Users.BuildConfiguration, "net452");

            try
            {
                toCopy.Do(tc =>
                {
                    FileInfo backup = new(Path.Combine(_loc.MagickaExe.DirectoryName, $"{tc}.bak"));

                    if (!backup.Exists)
                    {
                        Console.Write($"Backing up {tc}... ");
                        CopyRetry(Path.Combine(_loc.MagickaExe.DirectoryName, tc), backup.FullName);

                        Console.WriteLine("Done.");
                    }

                    Console.Write($"Copying new {tc}... ");
                    CopyRetry(Path.Combine(from, tc), Path.Combine(_loc.MagickaExe.DirectoryName, tc));
                    Console.WriteLine("Done.");
                });
            }
            finally
            {
                Cleanup(tempDir);
            }
        }

        private static void CopyRetry(string from, string to)
        {
            bool copied = false;
            int tries = 0;

            while (!copied && tries < 5)
            {
                try
                {
                    File.Copy(from, to, true);
                    copied = true;
                }
                catch (IOException)
                {
                    Console.Write('.');
                    tries++;

                    if (tries == 5)
                        throw;

                    Thread.Sleep(2_000);
                }
            }
        }


        private static void Cleanup(DirectoryInfo dir)
        {
            Console.Write("Deleting temporary Magicka folder... ");
            dir.Delete(true);
            Console.WriteLine("Done.");
        }
    }
}