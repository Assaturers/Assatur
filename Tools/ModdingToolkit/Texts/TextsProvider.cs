using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Webmilio.Commons.DependencyInjection;
using Webmilio.Commons.Extensions;

namespace ModdingToolkit.Texts
{
    [Service]
    public class TextsProvider : ITextsProvider
    {
        public async Task Load(Assembly assembly)
        {
            GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.PropertyType == typeof(string))
                .DoEnumerable(async p =>
                {
                    p.SetValue(this, await ReadText(assembly, p.Name));
                });
        }


        private static async Task<string> ReadText(Assembly assembly, string propertyName)
        {
            await using var stream = assembly.GetManifestResourceStream($"ModdingToolkit.Texts.{propertyName}.txt");
            using var reader = new StreamReader(stream);

            return await reader.ReadToEndAsync();
        }


        public string Wizard { get; private set; }
        public string Instructions { get; private set; }
    }
}