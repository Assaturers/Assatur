using System.IO;
using System.Threading.Tasks;

namespace ModdingToolkit.Patching
{
    public interface IPatcher
    {
        void Patch(DirectoryInfo patches, DirectoryInfo destination);
    }
}