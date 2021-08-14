using System;
using System.IO;
using System.Threading.Tasks;
using ModdingToolkit.Magicka;

namespace ModdingToolkit.Tools.Modding.Modder.Commands
{
    public class DecompileCommand : ModderCommand
    {
        private readonly IMagickaFinder _finder;
        private readonly IMagickaDecompiler _decompiler;

        public DecompileCommand(IMagickaFinder finder, IMagickaDecompiler decompiler)
        {
            _finder = finder;
            _decompiler = decompiler;
        }


        public override async Task Execute()
        {
            var path = await _finder.FindMagicka();
            Console.WriteLine($"Magicka installed on {path}");

            Console.Write("Where would you like to decompile? " );
            var dest = Console.ReadLine();

            if (!Directory.Exists(dest))
            {
                Console.WriteLine("Destination folder does not exist, creating!");
                Directory.CreateDirectory(dest);
            }

            Console.Write("Decompiling...");
            await _decompiler.DecompileFile(path, dest);

            Console.WriteLine("Done!");
        }

        public override string Name { get; } = "Decompile Magicka";
    }
}