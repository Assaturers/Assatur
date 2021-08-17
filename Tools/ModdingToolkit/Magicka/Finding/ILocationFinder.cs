using System.Threading.Tasks;

namespace ModdingToolkit.Magicka.Finding
{
    public interface ILocationFinder
    {
        Task<string> PlatformSearch();
    }
}