using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using DiffPatch;
using ModdingToolkit.Diffing;
using Webmilio.Commons.DependencyInjection;
using Webmilio.Commons.Extensions;

namespace ModdingToolkit.Patching
{
    [Service]
    public class StandardPatcher : IPatcher
    {
        public async Task Patch(DirectoryInfo destination, DirectoryInfo patches)
        {
            var files = patches.GetFiles("*.patch*", SearchOption.AllDirectories);
            List<Task> tasks = new(files.Length);

            files.Do(f => tasks.Add(Patch(destination, patches, f)));

            await Task.WhenAll(tasks);
        }

        public static async Task Patch(DirectoryInfo destination, DirectoryInfo patches, FileInfo patch)
        {
            var shortName = patch.FullName.Substring(patches.FullName.Length + 1);

            var destFolder = destination.Combine(Path.GetDirectoryName(shortName));
            destFolder.Create();

            var extension = Path.GetExtension(shortName);
            var destFile = new FileInfo(destFolder.CombineString(Path.GetFileNameWithoutExtension(shortName)));

            Console.WriteLine($"Applying patch {patch.Name}");

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
    }
}