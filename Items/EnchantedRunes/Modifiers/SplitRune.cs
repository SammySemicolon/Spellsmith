namespace Spellsmith.Items.EnchantedRunes.Modifiers
{
	public class SplitRune : ModifierRune
	{
		public override void SetStaticDefaults() 
		{
			Tooltip.SetDefault("Split");
		}
		public override void setup(Effect effect)
		{
			effect.split++;
			effect.damageMultiplier *= 0.6f;
			effect.accuracy *= 0.9f;
		}
	}
}