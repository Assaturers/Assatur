using System;
using System.IO;
using System.Threading.Tasks;
using ModdingToolkit.Magicka.Decompiling;
using ModdingToolkit.Magicka.Finding;

namespace ModdingToolkit.Tools.Modding.Modder.Commands
{
    public class DecompileCommand : ModderCommand
    {
        private readonly ILocationStore _loc;
        private readonly IDecompiler _decompiler;

        public DecompileCommand(ILocationStore loc, IDecompiler decompiler)
        {
            _loc = loc;
            _decompiler = decompiler;
        }


        public override async Task Execute()
        {
            Console.Write("Where would you like to decompile? " );
            var dest = Console.ReadLine();

            if (!Directory.Exists(dest))
            {
                Console.WriteLine("Destination folder does not exist, creating!");
                Directory.CreateDirectory(dest);
            }

            Console.Write("Decompiling...");
            await _decompiler.DecompileFile(_loc.MagickaExe.FullName, dest);

            Console.WriteLine("Done!");
        }

        public override string Name { get; } = "Decompile Magicka";
    }
}