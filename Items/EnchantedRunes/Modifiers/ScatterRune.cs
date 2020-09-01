namespace Spellsmith.Items.EnchantedRunes.Modifiers
{
	public class ScatterRune : ModifierRune
	{
		public override void SetStaticDefaults() 
		{
			Tooltip.SetDefault("Scatter");
		}
		public override void setup(Effect effect)
		{
			effect.scatter++;
			effect.damageMultiplier *= 0.6f;
		}
    }
}