using Magicka.GameLogic.GameStates;

namespace Magicka.ModLoader.Globals;

public class PlayStateGlobal : IPlayStateGlobal
{
    public bool PreSetDiedInLevel(PlayState state) => true;
    public void PostSetDiedInLevel(PlayState state) { }

    public virtual bool InstancePer { get; }
}