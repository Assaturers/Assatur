using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Win32;
using Webmilio.Commons.DependencyInjection;
using Webmilio.Commons.Extensions;

namespace ModdingToolkit.Magicka.Finding
{
    [Service]
    public class LocationStore : ILocationStore
    {
        public LocationStore()
        {
            MagickaExecutable = new FileInfo(PlatformSearch().Result);

            FileInfo backupMagicka = new(Path.Combine(MagickaExecutable.DirectoryName, Constants.MagickaExecutableBackup));

            if (backupMagicka.Exists)
                MagickaExecutable = backupMagicka;

            Assatur = MagickaExecutable.Directory.Combine(Constants.AssaturName);
            ModLoader = Assatur.Combine("ModLoader");

            DecompiledMagicka = ModLoader.Combine(Constants.Magicka);
            DecompiledAssatur = ModLoader.Combine(Constants.AssaturName);
            Patches = ModLoader.Combine(Constants.PatchesFolder);
        }


        public Task EnsureIntegrity()
        {
            Assatur.Create();
            ModLoader.Create();

            DecompiledMagicka.Create();
            DecompiledAssatur.Create();
            Patches.Create();

            return Task.CompletedTask;
        }


        public Task<string> PlatformSearch()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return WindowsSearch();
            }
            else
            {
                return LinuxInstallPath();
            }
        }

#pragma warning disable CA1416 // Validate platform compatibility

        // Windows Search
        private async Task<string> WindowsSearch()
        {
            string steamLocation = FindSteamInstallLocation();

            var defaultInstallPath = Path.Combine(steamLocation, "steamapps");
            var paths = ParseVDFForPaths(await File.ReadAllLinesAsync(
                    Path.Combine(defaultInstallPath, "libraryfolders.vdf")));

            for (int i = 0; i < paths.Count; i++)
            {
                paths[i] = Path.Combine(paths[i], "steamapps");
            }

            if (paths.Count > 0)
            {
                paths.Insert(0, defaultInstallPath);
            }
            else
            {
                paths.Add(defaultInstallPath);
            }

            string magickaLibrary = default;

            for (int i = 0; i < paths.Count && magickaLibrary == default; i++)
            {
                if (CheckForMagickaInLibrary(paths[i]))
                    magickaLibrary = paths[i];
            }

            if (magickaLibrary == default)
            {
                throw new ExecutionException(ExitCodes.MagickaFolderNotFound,
                    "Magicka not found!\n" +
                    "Please make sure that the Steam version of Magicka is installed before running the installer!");
            }

            var magickaPath = FindMagickaExe(magickaLibrary);

            if (magickaPath == default)
            {
                throw new ExecutionException(ExitCodes.MagickaExecutableNotFound,
                    $"Could not find {Constants.MagickaExecutable}!\n" +
                    "You might want to verify your game integrity.");
            }

            return magickaPath;
        }

        public string FindSteamInstallLocation()
        {
            string keyPath = Environment.Is64BitOperatingSystem ? @"SOFTWARE\WOW6432Node\Valve\Steam" : @"SOFTWARE\Valve\Steam";
            using var key = Registry.LocalMachine.OpenSubKey(keyPath);

            if (key == default)
            {
                throw new ExecutionException(ExitCodes.SteamNotFound,
                    "Could not find Steam?\n" +
                    "Please make sure Steam is installed before running the installer!");
            }

            var installPath = key.GetValue("InstallPath");

            if (installPath == default)
            {
                throw new ExecutionException(ExitCodes.SteamInstallPathNotFound,
                    "Steam registry key found but InstallPath was not?\n" +
                    "This error is unexpected, please report it to the support team.");
            }

            return installPath.ToString();
        }
#pragma warning restore CA1416 // Validate platform compatibility

        public List<string> ParseVDFForPaths(string[] lines)
        {
            const string pathKey = "\"path\"";
            List<string> found = new();

            foreach (var line in lines)
            {
                int keyBegin = line.IndexOf(pathKey);

                if (keyBegin == -1)
                    continue;

                keyBegin += pathKey.Length;

                var pathBegin = line.IndexOf('"', keyBegin);
                var path = line.Substring(pathBegin + 1, line.Length - pathBegin - 2);

                path = path.Replace(@"\\", @"\");

                found.Add(path);
            }

            return found;
        }

        public bool CheckForMagickaInLibrary(string library)
        {
            return File.Exists(Path.Combine(library, $"appmanifest_{Program.MagickaAppId}.acf"));
        }

        public string FindMagickaExe(string library)
        {
            var presumed = Path.Combine(library, "common", "Magicka", "Magicka.exe");

            return File.Exists(presumed) ? presumed : default;
        }

        // Linux Stuff
        private static async Task<string> LinuxInstallPath()
        {
            string path = default;

            while (string.IsNullOrWhiteSpace(path))
            {
                Console.Write("Please specify the install path to Magicka: ");
                path = Console.ReadLine();
            }

            return path;
        }

        public FileInfo MagickaExecutable { get; }

        public DirectoryInfo Assatur { get; }
        public DirectoryInfo ModLoader { get; }
        
        public DirectoryInfo DecompiledMagicka { get; }
        public DirectoryInfo DecompiledAssatur { get; }
        public DirectoryInfo Patches { get; }
    }
}