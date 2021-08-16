using System.Threading.Tasks;

namespace ModdingToolkit.Building
{
    public interface IBuilder
    {
        Task Build(string projectPath);
    }
}