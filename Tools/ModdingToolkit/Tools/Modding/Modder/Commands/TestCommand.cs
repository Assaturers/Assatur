using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using ModdingToolkit.Magicka.Finding;
using ModdingToolkit.Source;
using Webmilio.Commons.Extensions;

namespace ModdingToolkit.Tools.Modding.Modder.Commands
{
    public class TestCommand : ModderCommand
    {
        private readonly ILocationStore _loc;
        private readonly ISourceProvider _provider;

        public TestCommand(ILocationStore loc, ISourceProvider provider)
        {
            _loc = loc;
            _provider = provider;
        }

        public override async Task Execute()
        {
            DirectoryInfo dir = new(Path.Combine(Path.GetTempPath(), "Magicka"));
            dir.Recreate(true);

            await _provider.DownloadSource(dir);

            var extracted = dir.GetDirectories()[0];
            var patches = extracted.Combine("Assatur", "ModLoader", "Patches");

            Console.WriteLine("Patches extracted into {0}", patches);

            Debugger.Break();

            dir.Delete(true);
        }

        public override string Name { get; } = "Test";
    }
}