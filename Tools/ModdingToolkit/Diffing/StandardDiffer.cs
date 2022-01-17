using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DiffPatch;
using Webmilio.Commons.Console;
using Webmilio.Commons.DependencyInjection;
using Webmilio.Commons.Extensions;

namespace ModdingToolkit.Diffing
{
    [Service]
    public class StandardDiffer : IDiffer
    {
        public const string
            PatchExtension = ".patch",
            DeleteExtension = ".d",
            CreateExtension = ".c";

        public Task DiffFolders(DirectoryInfo origin, DirectoryInfo updated, DirectoryInfo patches)
        {
            var originalFiles = Directory.GetFiles(origin.FullName, "*.*", SearchOption.AllDirectories);
            var updatedFiles = Directory.GetFiles(updated.FullName, "*.*", SearchOption.AllDirectories);

            IList<string> toCreate, toDiff, toDelete;

            {
                var strippedOriginal = SelectFilter(originalFiles, origin);
                var strippedUpdated = SelectFilter(updatedFiles, updated);

                toDiff = strippedOriginal;
                toCreate = strippedUpdated.Where(su => !strippedOriginal.Any(so => so.Equals(su, StringComparison.OrdinalIgnoreCase))).ToArray();
                toDelete = strippedOriginal.Where(so => !strippedUpdated.Any(su => su.Equals(so, StringComparison.OrdinalIgnoreCase))).ToArray();
            }

            patches.Recreate(true);

            List<Thread> threads = new(3);

            if (toDiff.Count > 0)
            {
                threads.Add(
                    new Thread(async () =>
                {

                    var differ = new LineMatchedDiffer();
                    await toDiff.DoEnumerableAsync(p => Diff(differ, origin.FullName, updated.FullName, patches.FullName, p));
                }));
            }

            if (toCreate.Count > 0)
            {
                threads.Add(
                    new Thread(async () =>
                        await toCreate.DoAsync(p => WriteCreatePatch(updated.FullName, patches.FullName, p))));
            }

            if (toDelete.Count > 0)
            {
                threads.Add(
                    new Thread(async () =>
                        await toDelete.DoAsync(p => WriteDeletePatch(patches.FullName, p))));
            }

            threads.Do(t => t.Start());
            threads.Do(t => t.Join());

            return Task.CompletedTask;
        }

        private List<string> SelectFilter(string[] collection, DirectoryInfo root)
        {
            List<string> items = new(collection.Length);

            collection.Do(i =>
            {
                var n = StripPath(i, root.FullName);

                if (n.StartsWith('.') || n.StartsWith("bin") || n.StartsWith("obj"))
                    return;

                items.Add(n);
            });

            return items;
        }

        private string StripPath(string path, string root)
        {
            return path.Remove(0, root.Length + 1);
        }


        private async Task Diff(Differ differ, string originalRoot, string destinationRoot, string patchRoot, string shortName)
        {
            var destinationPath = Path.Combine(destinationRoot, shortName);

            if (!File.Exists(destinationPath))
                return;

            var diff = differ.DiffFile(Path.Combine(originalRoot, shortName), destinationPath,
                numContextLines: 0, includePaths: false);

            if (!diff.IsEmpty)
            {
                shortName += PatchExtension;
                await WriteDiffPatch(patchRoot, shortName, diff.ToString());
            }
        }


        private async Task WriteDiffPatch(string destRoot, string file, string content)
        {
            var dst = Path.Combine(destRoot, file);

            await WritePatch(dst, file, dst,
                async (s, d) => await File.WriteAllTextAsync(s, content));
        }

        private async Task WriteCreatePatch(string destRoot, string patchesRoot, string file)
        {
            await WritePatch(Path.Combine(destRoot, file), file, Path.Combine(patchesRoot, $"{file}{PatchExtension}{CreateExtension}"),
                (s, d) => Task.Run(() => File.Copy(s, d)));
        }

        private async Task WriteDeletePatch(string patchesRoot, string file)
        {
            await WritePatch(Path.Combine(patchesRoot, file), file, Path.Combine(patchesRoot, $"{file}{PatchExtension}{DeleteExtension}"),
                (s, d) => Task.Run(() => File.Create(d).Close()));
        }

        private static async Task WritePatch(string src, string displayPath, string dest, Func<string, string, Task> action)
        {
            Console.WriteLine("Creating patch {0}... ", displayPath);

            try
            {
                // Find the first directory that exists so that we can create it backwards!
                // I can't see this going wrong at all 🙂🙂🙂
                if (dest != null)
                    ValidateDirectoryPath(new(Path.GetDirectoryName(dest)));

                await action(src, dest);
            }
            catch (Exception e)
            {
                ConsoleHelper.WriteLineError("Failed creating patch for {0}:\n{1}.", displayPath, e);
            }
        }

        private static void ValidateDirectoryPath(DirectoryInfo dir)
        {
            if (!dir.Parent.Exists)
                ValidateDirectoryPath(dir.Parent);

            dir.Create();
        }
    }
}