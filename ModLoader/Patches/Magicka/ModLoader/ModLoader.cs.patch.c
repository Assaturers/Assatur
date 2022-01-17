using System.Collections.Generic;
using System.Reflection;
using Webmilio.Commons.Assemblies;

namespace Magicka.ModLoader;

public class ModLoader
{
    private class ModAssembliesResolver : IAssemblyResolver
    {
        public IList<Assembly> Resolve()
        {

        }
    }
}