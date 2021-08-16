using System.IO;
using System.Threading.Tasks;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.CSharp.ProjectDecompiler;
using ICSharpCode.Decompiler.Metadata;
using Webmilio.Commons.DependencyInjection;

namespace ModdingToolkit.Magicka.Decompiling
{
    [Service]
    public class Decompiler : IDecompiler
    {
        public Task DecompileFile(string from, string to)
        {
            if (!Directory.Exists(to))
                Directory.CreateDirectory(to);

            var module = new PEFile(from);
            var resolver = new UniversalAssemblyResolver(from, false, module.Reader.DetectTargetFrameworkId());

            var decompiler = new WholeProjectDecompiler(GetSettings(module), resolver, resolver, null);
            decompiler.DecompileProject(module, to);

            return Task.CompletedTask;
        }

        internal static DecompilerSettings GetSettings(PEFile module)
        {
            return new(LanguageVersion.Latest)
            {
                RemoveDeadCode = true,
                RemoveDeadStores = true,

                Ranges = false,

                ThrowOnAssemblyResolveErrors = false,
                UseSdkStyleProjectFormat = WholeProjectDecompiler.CanUseSdkStyleProjectFormat(module),
            };
        }
    }
}