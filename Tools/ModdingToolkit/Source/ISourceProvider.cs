using System.IO;
using System.Threading.Tasks;

namespace ModdingToolkit.Source
{
    public interface ISourceProvider
    {
        Task DownloadSource(DirectoryInfo dest);
    }
}
