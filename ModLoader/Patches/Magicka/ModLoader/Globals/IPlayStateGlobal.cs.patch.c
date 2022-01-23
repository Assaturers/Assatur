using Magicka.GameLogic.GameStates;

namespace Magicka.ModLoader.Globals;

public interface IPlayStateGlobal : IGlobal
{
    public bool PreSetDiedInLevel(PlayState state);
    public void PostSetDiedInLevel(PlayState state);
}