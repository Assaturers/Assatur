using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Win32;
using ModdingToolkit.Core;
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
                msBuild = Process.Start(MakeProcessInfo(root, GetMSBuildPath(), $"/property:Configuration={Constants.Users.BuildConfiguration}"));
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

#pragma warning disable CA1416 // Validate platform compatibility
        public static string GetMSBuildPath()
        {
            using var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\MSBuild\4.0");

            return key.GetValue("MSBuildOverrideTasksPath").ToString();
        }
#pragma warning restore
    }
}