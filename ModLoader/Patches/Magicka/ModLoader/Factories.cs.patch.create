using Magicka.Extensions;
using Magicka.ModLoader.Globals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Magicka.ModLoader;

public abstract class Factory<T>
{
    protected virtual T Make(Type type)
    {
        return (T)Activator.CreateInstance(type);
    }

    protected virtual List<T> Make(IEnumerable<Type> types)
    {
        var instances = new List<T>();

        foreach (var type in types)
            instances.Add(Make(type));

        return instances;
    }
}

public class GlobalFactory<TSource, TDestination> : Factory<TDestination> where TDestination : Global
{
    private readonly TDestination[] _prototypes, _nonInstanced, _instanced;

    public GlobalFactory(ReadOnlyCollection<Mod> mods)
    {
        Mods = mods;

        var prototypes = Make(GetDestinationTypes().Flatten());

        var nonInstanced = new List<TDestination>();
        var instanced = new List<TDestination>();

        for (int i = prototypes.Count - 1; i >= 0; i--)
        {
            (prototypes[i].InstancePer ? instanced : nonInstanced).Add(prototypes[i]);
        }

        _prototypes = prototypes.ToArray();
        _nonInstanced = nonInstanced.ToArray();
        _instanced = instanced.ToArray();
    }

    protected IEnumerable<List<Type>> GetDestinationTypes()
    {
        foreach (var mod in Mods)
        {
            yield return mod.Assembly.GetTypes().ConcreteSubclasses<TDestination>();
        }
    }

    public virtual List<TDestination> Make(TSource source)
    {
        var instances = new List<TDestination>();

        instances.AddRange(_nonInstanced);
        instances.AddRange(Make(_instanced.Select(i => i.GetType())));

        return instances;
    }

    protected ReadOnlyCollection<Mod> Mods { get; }
}