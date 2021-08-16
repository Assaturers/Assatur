using System.IO;
using System.Threading.Tasks;

namespace ModdingToolkit.Magicka.Finding
{
    public interface ILocationStore
    {
        Task EnsureIntegrity();

        FileInfo MagickaExecutable { get; }

       DirectoryInfo Assatur { get; }
       DirectoryInfo ModLoader { get; }

       DirectoryInfo DecompiledAssatur { get; }
       DirectoryInfo DecompiledMagicka { get; }
       DirectoryInfo Patches { get; }
    }
}