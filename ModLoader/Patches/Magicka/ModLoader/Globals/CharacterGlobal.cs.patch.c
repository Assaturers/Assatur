namespace Magicka.ModLoader.Globals;

public class CharacterGlobal
{
    public virtual bool PreGetSpellRangeModifier(ref float range) => true;
    public virtual void GetSpellRangeModifier(ref float range) { }

    public virtual bool PreGetAggroMultiplier(ref float multiplier) => true;
    public virtual void GetAggroMultiplier(ref float multiplier) { }

    public virtual bool PreDamage(ref float damage, ref Elements element) => true;
    public virtual void Damage(ref float damage, ref Elements element) { }
}
