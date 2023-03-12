using Magicka.ModLoader.Globals;
using System.Collections.Generic;

namespace Magicka.GameLogic.Spells;

public partial struct Spell
{
    private readonly IList<SpellGlobal> _globals;

    public Spell()
    {
        _globals = Game.Instance.ModLoader.SpellGlobalFactory.Make(this);
    }
}
