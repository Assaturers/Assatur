using System.IO;
using System.Threading.Tasks;

namespace ModdingToolkit.Diffing
{
    public interface IDiffer
    {
        Task DiffFolders(DirectoryInfo origin, DirectoryInfo updated, DirectoryInfo patches);
    }
}