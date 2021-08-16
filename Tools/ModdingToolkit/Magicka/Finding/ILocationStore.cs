using System.IO;
using System.Threading.Tasks;

namespace ModdingToolkit.Magicka.Finding
{
    public interface ILocationStore
    {
        Task EnsureIntegrity();
        void RestoreBackup();

        FileInfo MagickaExe { get; }
        FileInfo MagickaConfig { get; }

        DirectoryInfo Assatur { get; }
        DirectoryInfo ModLoader { get; }

        DirectoryInfo DecompAssatur { get; }
        DirectoryInfo DecompMagicka { get; }
        DirectoryInfo Patches { get; }
    }
}