using System;
using System.IO;
using System.Threading.Tasks;
using ModdingToolkit.Magicka;
using ModdingToolkit.Magicka.Decompiling;
using ModdingToolkit.Magicka.Finding;
using ModdingToolkit.Patching;
using ModdingToolkit.Texts;
using Webmilio.Commons.Console;
using Webmilio.Commons.Extensions;

namespace ModdingToolkit.Tools.Modding.Modder.Commands
{
    public class SetupCommand : ModderCommand
    {
        private readonly ILocationStore _loc;
        private readonly IDecompiler _decompiler;
        private readonly IPatcher _patcher;
        private readonly TextsProvider _texts;

        public SetupCommand(ILocationStore loc, IDecompiler decompiler, IPatcher patcher, TextsProvider texts)
        {
            _loc = loc;
            _decompiler = decompiler;
            _patcher = patcher;
            _texts = texts;
        }

        public override async Task Execute()
        {
            var magickaExe = _loc.MagickaExecutable;
            await _loc.EnsureIntegrity();

            _loc.DecompiledMagicka.Recreate(true);
            _loc.DecompiledAssatur.Recreate(true);
            _loc.Patches.Recreate(true); // Wait for patch download

            Console.Write("Decompiling Magicka, this can take a long time (especially if Magicka is not on fast storage)... ");
            await _decompiler.DecompileFile(magickaExe.FullName, _loc.DecompiledMagicka.FullName);
            Console.WriteLine("Done.");

            Console.Write("Cloning decompiled Magicka... ");
            _loc.DecompiledMagicka.CopyTo(_loc.DecompiledAssatur, true);
            Console.WriteLine("Done.");

            Console.Write("Validating project... ");
            Console.WriteLine("Done.");

            Console.WriteLine("Applying patches to Assatur... ");
            await StandardPatcher.StandardPatch(_patcher, _loc);

            DirectoryInfo debugTarget = _loc.DecompiledAssatur
                    .CreateSubdirectory("bin")
                    .CreateSubdirectory("Debug")
                    .CreateSubdirectory("net452");

            Console.WriteLine("Copying all required files and folder into target debug folder...");
            _texts.SetupDebugCopy.Split(Environment.NewLine).Do(s =>
            {
                Console.Write($"Copying {s}... ");

                try
                {
                    if (s.EndsWith('/'))
                    {
                        DirectoryInfo
                            from = _loc.MagickaExecutable.Directory.Combine(s),
                            to = debugTarget.Combine(s);

                        from.CopyTo(to, true);
                    }
                    else
                    {
                        string
                            from = Path.Combine(_loc.MagickaExecutable.DirectoryName, s),
                            to = Path.Combine(debugTarget.FullName, s);

                        File.Copy(from, to);
                    }
                    Console.WriteLine("Done.");
                }
                catch
                {
                    ConsoleHelper.WriteLineError("Failed!");
                }
            });
        }

        public override string Name { get; } = "Setup Environment";
    }
}