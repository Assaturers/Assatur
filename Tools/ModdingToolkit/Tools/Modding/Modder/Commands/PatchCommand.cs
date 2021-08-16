using System;
using System.Threading.Tasks;
using ModdingToolkit.Magicka.Finding;
using ModdingToolkit.Patching;

namespace ModdingToolkit.Tools.Modding.Modder.Commands
{
    public class PatchCommand : ModderCommand
    {
        private readonly ILocationStore _loc;
        private readonly IPatcher _patcher;

        public PatchCommand(ILocationStore loc, IPatcher patcher)
        {
            _loc = loc;
            _patcher = patcher;
        }

        public override async Task Execute()
        {
            Console.WriteLine("Applying patches to Assatur... ");
            await StandardPatcher.StandardPatch(_patcher, _loc);
        }

        public override string Name { get; } = "Apply Patches";
    }
}