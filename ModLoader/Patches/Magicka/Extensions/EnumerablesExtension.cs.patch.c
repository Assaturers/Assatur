using System;
using System.Collections.Generic;

namespace Magicka.Extensions;

public static class EnumerablesExtension
{
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