using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DiffPatch;
using ModdingToolkit.Diffing;
using ModdingToolkit.Magicka.Finding;
using Webmilio.Commons.DependencyInjection;
using Webmilio.Commons.Extensions;

namespace ModdingToolkit.Patching
{
    [Service]
    public class StandardPatcher : IPatcher
    {
        public Task Patch(DirectoryInfo patches, DirectoryInfo destination)
        {
            var files = patches.GetFiles("*.patch*", SearchOption.AllDirectories);

            if (files.Length == 0)
                return Task.CompletedTask;

            var tasks = new List<Task>();
            files.Do(f => tasks.Add(Task.Run(async () => await Patch(destination, patches, f))));

            return Task.WhenAll(tasks.ToArray());
        }

        public static async Task Patch(DirectoryInfo destination, DirectoryInfo patches, FileInfo patch)
        {
            var shortName = patch.FullName.Substring(patches.FullName.Length + 1);

            var destFolder = destination.Combine(Path.GetDirectoryName(shortName));
            destFolder.Create();

            var extension = Path.GetExtension(shortName);
            var destFile = new FileInfo(destFolder.CombineString(Path.GetFileNameWithoutExtension(shortName)));

            Console.WriteLine($"Applying {shortName}");

            if (extension.Equals(StandardDiffer.PatchExtension))
            {
                // This is probably inefficient and could be replaced with Streams (Readers/Writers) but the current
                // implementation of DiffPatch kinda sucks :/
                var patchFile = FilePatcher.FromPatchFile(patch.ToString());
                var lines = new Patcher(patchFile.PatchFile.Patches, await File.ReadAllLinesAsync(destFile.ToString()))
                    .Patch(default)
                    .ResultLines;

                await File.WriteAllLinesAsync(destFile.FullName, lines);
            }
            else
            {
                destFile = new FileInfo(Path.Combine(
                        destFile.DirectoryName,
                        Path.GetFileNameWithoutExtension(destFile.Name)));

                if (extension.Equals(StandardDiffer.CreateExtension))
                {
                    patch.CopyTo(destFile.ToString());
                }
                else if (extension.Equals(StandardDiffer.DeleteExtension))
                {
                    if (destFile.Exists) 
                    {
                        destFile.Delete();
                    }
                }
            }
        }

        public static Task StandardPatch(IPatcher patcher, ILocationStore loc)
        {
            return patcher.Patch(loc.Patches, loc.DecompAssatur);
        }
    }
}