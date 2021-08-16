using System;
using System.Threading.Tasks;
using ModdingToolkit.Diffing;
using ModdingToolkit.Magicka.Finding;

namespace ModdingToolkit.Tools.Modding.Modder.Commands
{
    public class DiffCommand : ModderCommand
    {
        private readonly ILocationStore _loc;
        private readonly IDiffer _differ;
        
        public DiffCommand(ILocationStore loc, IDiffer differ)
        {
            _loc = loc;
            _differ = differ;
        }

        public override async Task Execute()
        {
            var patches = _loc.Patches;

            patches.Delete(true);
            patches.Create();

            Console.WriteLine("Diffing...");
            await _differ.DiffFolders(_loc.DecompMagicka, _loc.DecompAssatur, patches);
            Console.WriteLine("Done.");
        }

        public override string Name { get; } = "Create Patches (Diff)";
    }
}