using System.IO;
using System.Threading.Tasks;

namespace ModdingToolkit.Patching
{
    public interface IPatcher
    {
        Task Patch(DirectoryInfo patches, DirectoryInfo destination);
    }
}