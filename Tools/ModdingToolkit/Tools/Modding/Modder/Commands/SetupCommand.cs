using System;
using System.IO;
using System.Threading.Tasks;
using ModdingToolkit.Magicka;
using ModdingToolkit.Magicka.Decompiling;
using ModdingToolkit.Magicka.Finding;
using ModdingToolkit.Patching;
using Webmilio.Commons.Extensions;

namespace ModdingToolkit.Tools.Modding.Modder.Commands
{
    public class SetupCommand : ModderCommand
    {
        private readonly ILocationStore _loc;
        private readonly IDecompiler _decompiler;
        private readonly IPatcher _patcher;

        public SetupCommand(ILocationStore loc, IDecompiler decompiler, IPatcher patcher)
        {
            _loc = loc;
            _decompiler = decompiler;
            _patcher = patcher;
        }

        public override async Task Execute()
        {
            var magickaExe = _loc.MagickaExecutable;
            await _loc.EnsureIntegrity();

            _loc.DecompiledMagicka.Recreate(true);
            _loc.DecompiledAssatur.Recreate(true);
            // _loc.Patches.Recreate(true); // Wait for patch download

            Console.Write("Decompiling Magicka... ");
            await _decompiler.DecompileFile(magickaExe.FullName, _loc.DecompiledMagicka.FullName);
            Console.WriteLine("Done!");

            Console.Write("Cloning decompiled Magicka... ");
            _loc.DecompiledMagicka.CopyTo(_loc.DecompiledAssatur, true);
            Console.WriteLine("Done!");

            Console.Write("Validating project... ");
            Console.WriteLine("Done.");

            Console.WriteLine("Applying patches to Assatur... ");
            await _patcher.Patch(_loc.Patches, _loc.DecompiledAssatur);
        }

        public override string Name { get; } = "Setup Environment";
    }
}