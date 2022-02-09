using System;
using Webmilio.Commons.DependencyInjection;
using Webmilio.Commons.Loaders;

namespace ModdingToolkit.Tools.Modding.Modder.Commands
{
    [Service]
    public class ModderCommandLoader : PrototypeLoader<ModderCommand>
    {
        private readonly IServiceProvider _services;


        public ModderCommandLoader(IServiceProvider services)
        {
            _services = services;
            Initialize();
        }


        public override ModderCommand Create(Type type)
        {
            return _services.Make(type) as ModderCommand;
        }
    }
}