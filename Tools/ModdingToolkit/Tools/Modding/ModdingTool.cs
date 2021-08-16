using System.Linq;
using System.Threading.Tasks;
using ModdingToolkit.Tools.Modding.Modder.Commands;
using Webmilio.Commons.DependencyInjection;

namespace ModdingToolkit.Tools.Modding
{
    [Service]
    public class ModdingTool : IModdingTool
    {
        private readonly ModderCommandLoader _commands;


        public ModdingTool(ModderCommandLoader commands)
        {
            _commands = commands;
        }


        public async Task Execute()
        {
            bool running = true;
            var commands = _commands.AsEnumerable().ToArray();

            while (running)
            {
                running = InputHelper.Choose(commands, out var command);

                if (running)
                {
                    await command.Execute();
                }
            }
        }
    }
}