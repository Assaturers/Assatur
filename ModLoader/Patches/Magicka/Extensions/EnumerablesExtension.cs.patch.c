using System;
using System.Collections.Generic;

namespace Magicka.Extensions;

public static class EnumerablesExtension
{
    public static bool TrueForAll<T>(this T[] source, Predicate<T> predicate)
    {
        for (int i = 0; i < source.Length; i++)
            if (!predicate(source[i]))
                return false;

        return true;
    }

    public static void Do<T>(this IList<T> source, Action<T> action)
    {
        for (int i = 0; i < source.Count; i++)
            action(source[i]);
    }

    public static void Do<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (var item in source)
            action(item);
    }
}