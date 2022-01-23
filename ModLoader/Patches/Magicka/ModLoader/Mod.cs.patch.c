using System.Collections.Generic;
using System.Reflection;
using Magicka.Logging;
using Magicka.ModLoader.Globals;

namespace Magicka.ModLoader;

public abstract class Mod
{
    public virtual void LoadContent() { }
    public virtual void PostLoadContent() { }

    public Logger Logger { get; internal set; }
    public Assembly Assembly { get; internal set; }
}