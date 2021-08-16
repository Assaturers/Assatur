using System.Threading.Tasks;
using ModdingToolkit.Magicka;
using ModdingToolkit.Magicka.Installing;
using Webmilio.Commons.DependencyInjection;

namespace ModdingToolkit.Tools.User
{
    [Service]
    public class UserTool : IUserTool
    {
        private readonly IAssaturInstaller _installer;

        public UserTool(IAssaturInstaller installer)
        {
            _installer = installer;
        }

        public async Task Execute()
        {
            await _installer.Install();
        }
    }
}