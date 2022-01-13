using System.IO;

namespace ModdingToolkit.AssetsExtractor
{
    public static class Extensions
    {
        public static FileInfo Combine(this FileInfo file, params string[] paths) => new FileInfo(CombineString(file, paths));
        public static DirectoryInfo Combine(this DirectoryInfo directory, params string[] paths) => new DirectoryInfo(CombineString(directory, paths));

        public static string CombineString(this FileSystemInfo fsInfo, params string[] paths)
        {
            string path;

            if (paths.Length == 1)
                path = Path.Combine(fsInfo.FullName, paths[0]);
            else
                path = fsInfo.FullName + Path.Combine(paths);

            return path;
        }

        #region Recreate

        public static void Recreate(this DirectoryInfo directory) => Recreate(directory, false);

        public static void Recreate(this DirectoryInfo directory, bool recursiveDelete)
        {
            if (directory.Exists)
                directory.Delete(recursiveDelete);

            directory.Create();
        }

        #endregion

        #region Folder Copy
        // Expertly stolen from https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-copy-directories
        public static void CopyTo(this DirectoryInfo from, string to) => CopyTo(from, to, false);
        public static void CopyTo(this DirectoryInfo from, string to, bool copySubDirectories) => CopyTo(from, new DirectoryInfo(to), copySubDirectories);
        public static void CopyTo(this DirectoryInfo from, DirectoryInfo to) => CopyTo(from, to, false);

        public static void CopyTo(this DirectoryInfo from, DirectoryInfo to, bool copySubDirectories)
        {
            if (!from.Exists)
                throw new DirectoryNotFoundException($"Directory `{from}` does not exist or could not be found.");

            to.Create(); // Does nothing if it already exists.

            foreach (var file in from.EnumerateFiles())
                file.CopyTo(to.CombineString(file.Name));

            if (copySubDirectories)
                foreach (var d in from.EnumerateDirectories())
                    d.CopyTo(to.Combine(d.Name), true);
        }
        #endregion
    }
}