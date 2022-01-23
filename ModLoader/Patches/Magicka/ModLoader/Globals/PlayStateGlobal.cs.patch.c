namespace Magicka.ModLoader.Globals;

public class PlayStateGlobal : IPlayStateGlobal
{
    public bool PreSetDiedInLevel() => true;
    public void PostSetDiedInLevel() { }

    public virtual bool InstancePer { get; }
}