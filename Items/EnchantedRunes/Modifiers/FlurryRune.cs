namespace Spellsmith.Items.EnchantedRunes.Modifiers
{
	public class FlurryRune : ModifierRune
	{
		public override void SetStaticDefaults() 
		{
			Tooltip.SetDefault("Flurry");
		}
		public override void setup(Effect effect)
		{
			effect.flurry++;
			effect.damageMultiplier *= 0.7f;
		}
	}
}