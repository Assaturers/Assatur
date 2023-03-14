using Magicka.Extensions;
using Magicka.GameLogic.Entities;
using Magicka.GameLogic.GameStates;
using Magicka.GameLogic.Spells;
using Magicka.ModLoader.Globals;
using System;
using System.Reflection;

namespace Magicka.ModLoader;

public partial class ModLoader
{
    [AttributeUsage(AttributeTargets.Property)]
    private class FactoryAttribute : Attribute { }

    private void InitializeFactories()
    {
        foreach (var property in GetType().GetProperties())
        {
            if (property.HasAttribute<FactoryAttribute>())
            {
                var factory = Activator.CreateInstance(property.PropertyType, new object[] { Mods });
                property.SetValue(this, factory);
            }
        }
    }

    [Factory] public GlobalFactory<PlayState, PlayStateGlobal> PlayStateGlobalFactory { get; private set; }
    [Factory] public GlobalFactory<Avatar, AvatarGlobal> AvatarGlobalFactory { get; private set; }
    [Factory] public GlobalFactory<Spell, SpellGlobal> SpellGlobalFactory { get; private set; }
}
