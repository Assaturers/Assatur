using Magicka.Logging;

namespace Magicka.ModLoader;

public abstract class Mod
{
    public virtual void LoadContent() { }

    public virtual void PostLoadContent() { }

    public Logger Logger { get; internal set; }
}