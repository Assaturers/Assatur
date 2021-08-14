using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DiffPatch;
using ModdingToolkit.Diffing;
using Webmilio.Commons.Console;
using Webmilio.Commons.Extensions;

namespace ModdingToolkit.Tools.Modding.Modder.Commands
{
    public class DiffCommand : ModderCommand
    {
        private readonly IDiffer _differ;
        
        public DiffCommand(IDiffer differ)
        {
            _differ = differ;
        }

        public override async Task Execute()
        {
            DirectoryInfo original, updated, patches;

#if DEBUG
            original = new(@"L:\Coding\Games\Modding\Clean");
            updated = new(@"L:\Coding\Games\Modding\Updated");
            patches = new(@"L:\Coding\Games\Modding\Patches");
#else
            Console.Write("Original Folder: ");
            original = Console.ReadLine();

            Console.Write("Updated Folder: ");
            updated = Console.ReadLine();

            Console.Write("Patches Output: ");
            patches = Console.ReadLine();
#endif

            if (patches.Exists)
                patches.Delete(true);

            patches.Create();

            await _differ.DiffFolders(original, updated, patches);
            Console.WriteLine();
        }

        public override string Name { get; } = "Create Patches";
    }
}