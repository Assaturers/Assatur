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
    public class LocationStore : ILocationStore
    {
        public ILocationStore SetLocations(string magickaPath)
        {
            MagickaExe = new FileInfo(magickaPath);
            MagickaConfig = new FileInfo($"{MagickaExe.FullName}.config");

            Assatur = MagickaExe.Directory.Combine(Constants.AssaturName);
            ModLoader = Assatur.Combine("ModLoader");

            DecompMagicka = ModLoader.Combine(Constants.Magicka);
            DecompAssatur = ModLoader.Combine(Constants.AssaturName);
            Patches = ModLoader.Combine(Constants.PatchesFolder);

            return this;
        }

        public Task EnsureIntegrity()
        {
            Assatur.Create();
            ModLoader.Create();

            DecompMagicka.Create();
            DecompAssatur.Create();
            Patches.Create();

            return Task.CompletedTask;
        }

        public void RestoreBackup()
        {
            FileInfo exeBackup = new($"{MagickaExe}{Constants.BackupExtension}");
            if (exeBackup.Exists)
            {
                MagickaExe.Delete();
                exeBackup.MoveTo(MagickaExe.FullName);
            }

            FileInfo configBackup = new($"{MagickaConfig}{Constants.BackupExtension}");
            if (configBackup.Exists)
            {
                MagickaConfig.Delete();
                configBackup.MoveTo(MagickaConfig.FullName);
            }
        }


        public FileInfo MagickaExe { get; private set; }
        public FileInfo MagickaConfig { get; private set; }

        public DirectoryInfo Assatur { get; private set; }
        public DirectoryInfo ModLoader { get; private set; }
        
        public DirectoryInfo DecompMagicka { get; private set; }
        public DirectoryInfo DecompAssatur { get; private set; }
        public DirectoryInfo Patches { get; private set; }
    }
}