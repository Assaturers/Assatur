using Magicka.GameLogic.Entities;

namespace Magicka.ModLoader.Globals;

public class AvatarGlobal : Global
{
    public virtual bool PreDie(Avatar avatar) => true;
    public virtual void PostDie(Avatar avatar) { }
}
