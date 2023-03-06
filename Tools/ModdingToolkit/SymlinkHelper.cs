using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Webmilio.Commons.DependencyInjection;

namespace ModdingToolkit;

[Service]
public class SymlinkHelper
{
    public void MakeSymlink(string src, string dst)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            ProcessStartInfo info = new()
            {
                FileName = "cmd",
                Arguments = $"/C mklink /D \"{dst}\" \"{src}\"",

                Verb = "runas",
                UseShellExecute = true,
            };

            Process.Start(info).WaitForExit();
        }
        else
        {
            File.CreateSymbolicLink(src, dst);
        }

        if (!File.Exists(src) || !Directory.Exists(src))
            throw new IOException("Creation of Symlink failed.");
    }
}