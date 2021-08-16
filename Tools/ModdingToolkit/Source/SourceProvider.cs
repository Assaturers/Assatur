using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using ModdingToolkit.GitHub;
using Webmilio.Commons.DependencyInjection;

namespace ModdingToolkit.Source
{
    [Service]
    public class SourceProvider : ISourceProvider
    {
        public async Task DownloadSource(DirectoryInfo dest)
        {
            new ZipArchive(await GitHubHelper.DownloadLatest())
                .ExtractToDirectory(dest.FullName);
        }
    }
}
