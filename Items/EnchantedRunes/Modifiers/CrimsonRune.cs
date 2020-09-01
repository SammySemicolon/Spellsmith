namespace Spellsmith.Items.EnchantedRunes.Modifiers
{
	public class CrimsonRune : ModifierRune
	{
		public override void SetStaticDefaults() 
		{
			Tooltip.SetDefault("Crimson");
		}
		public override void setup(Effect effect)
		{
			effect.crimson = true;
		}
	}
}