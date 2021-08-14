using System.Threading.Tasks;

namespace ModdingToolkit.Magicka
{
    public interface IMagickaDecompiler
    {
        Task DecompileFile(string from, string to);
    }
}