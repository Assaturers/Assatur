using Magicka.ModLoader.Globals;
using System.Collections.Generic;

namespace Magicka.GameLogic.GameStates;

public partial class PlayState
{
    private List<IPlayStateGlobal> globals = new();
}
