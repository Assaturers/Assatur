using System.Threading.Tasks;
using ModdingToolkit.Magicka;
using Webmilio.Commons.DependencyInjection;

namespace ModdingToolkit.Tools.User
{
    [Service]
    public class UserTool : IUserTool
    {
        private readonly IMagickaInstaller _installer;

        public UserTool(IMagickaInstaller installer)
        {
            _installer = installer;
        }

        public async Task Execute()
        {
            await _installer.Install();
        }
    }
}