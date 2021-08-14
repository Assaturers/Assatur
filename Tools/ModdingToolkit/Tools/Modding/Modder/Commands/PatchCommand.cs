using System;
using System.IO;
using System.Threading.Tasks;
using ModdingToolkit.Magicka;
using ModdingToolkit.Patching;

namespace ModdingToolkit.Tools.Modding.Modder.Commands
{
    public class PatchCommand : ModderCommand
    {
        private readonly IPatcher _patcher;

        public PatchCommand(IPatcher patcher)
        {
            _patcher = patcher;
        }

        public override async Task Execute()
        {
            var patches = InputHelper.EnterDirectory("Patches Root: ");
            var dest = InputHelper.EnterDirectory("Destionation Root: ");

            await _patcher.Patch(dest, patches);
        }

        public override string Name { get; } = "Apply Patches";
    }
}