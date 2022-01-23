using System;
using System.Collections.Generic;

namespace Magicka.Extensions;

public static class AssembliesExtension
{
    public static List<Type> Subclasses<T>(this IEnumerable<Type> types) => FindTypes<T>(types, IsSubclassOf<T>);

    public static List<Type> ConcreteSubclasses<T>(this IEnumerable<Type> types) => FindTypes<T>(types, IsConcreteSubclass<T>);

    public static List<Type> FindTypes<T>(this IEnumerable<Type> types, Predicate<Type> predicate)
    {
        List<Type> t = new();

        foreach (var type in types)
            if (predicate(type))
                t.Add(type);

        return t;
    }

    private static bool IsSubclassOf<T>(Type type) => type.IsSubclassOf(typeof(T));

    private static bool IsConcrete(Type type) => !type.IsAbstract && !type.IsInterface;

    private static bool IsConcreteSubclass<T>(Type type) => IsConcrete(type) && IsSubclassOf<T>(type);
}