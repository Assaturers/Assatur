using System;
using System.ComponentModel.Design;

namespace Magicka.Extensions;

public static class ServicesExtension
{
    public static T GetService<T>(this IServiceContainer services)
    {
        return (T) services.GetService(typeof(T));
    }
}