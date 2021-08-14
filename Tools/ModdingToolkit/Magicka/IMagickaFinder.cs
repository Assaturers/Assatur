using System.Threading.Tasks;

namespace ModdingToolkit.Magicka
{
    public interface IMagickaFinder
    {
       Task<string> FindMagicka();
    }
}