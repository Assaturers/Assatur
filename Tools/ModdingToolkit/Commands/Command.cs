using System.Threading.Tasks;

namespace ModdingToolkit.Commands
{
    public abstract class Command
    {
        public abstract Task Execute();


        public abstract string Name { get; }
    }
}