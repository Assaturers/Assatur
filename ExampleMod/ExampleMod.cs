using Magicka.Logging;
using Magicka.ModLoader;

namespace ExampleMod;

public class ExampleMod : Mod
{
    public override void LoadContent()
    {
        Logger.Assert(Logger.Source.ModLoader, "First mod loaded!!!!!!");
    }
}
