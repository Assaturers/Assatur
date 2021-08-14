using System.Reflection;
using System.Threading.Tasks;

namespace ModdingToolkit.Texts
{
    public interface ITextsProvider
    {
        Task Load(Assembly assembly);
    }
}