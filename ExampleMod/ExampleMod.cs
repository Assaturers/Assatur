using Magicka;
using Magicka.GameLogic.Entities;
using Magicka.GameLogic.GameStates;
using Magicka.Logging;
using Magicka.ModLoader;
using Magicka.ModLoader.Globals;

namespace ExampleMod;

public class ExampleMod : Mod
{
    public override void LoadContent()
    {
        
    }
}

public class EMGlobalAvatar : AvatarGlobal
{
    private int _extraLives = 2;

    public override bool PreDie(Avatar avatar)
    {
        if (_extraLives > 0)
        {
            //avatar.HasFairy = true;

            _extraLives--;
        }

        return true;
    }
}
