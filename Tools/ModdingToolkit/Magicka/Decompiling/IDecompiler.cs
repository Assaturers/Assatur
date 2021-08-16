using System.Threading.Tasks;

namespace ModdingToolkit.Magicka.Decompiling
{
    public interface IDecompiler
    {
        Task DecompileFile(string from, string to);
    }
}