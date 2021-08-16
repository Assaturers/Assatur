using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Webmilio.Commons.DependencyInjection;

namespace ModdingToolkit.Building
{
    [Service]
    public class MSBuildBuilder : IBuilder
    {
        public async Task Build(string projectPath)
        {
            var root = Path.GetDirectoryName(projectPath);

            await Process.Start(MakeProcessInfo(root,"dotnet", "restore")).WaitForExitAsync();
            
            Process msBuild;
            {
                msBuild = Process.Start(MakeProcessInfo(root, "msbuild", $"/property:Configuration={Constants.Users.BuildConfiguration}"));
                await msBuild.WaitForExitAsync();
            }

            if (msBuild.ExitCode > 0)
            {
                throw new ExecutionException(ExitCodes.BuildFailed, "Could not build patched Magicka.");
            }
        }

        private static ProcessStartInfo MakeProcessInfo(string root, string fileName, string args = default)
        {
            ProcessStartInfo info = new()
            {
                FileName = fileName,
                WorkingDirectory = root,
            };

            if (!string.IsNullOrWhiteSpace(args))
            {
                info.Arguments = args;
            }

            return info;
        }
    }
}