namespace Spellsmith.Items.EnchantedRunes.Modifiers
{
	public class FrostRune : ModifierRune
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Frost");
		}
		public override void setup(Effect effect)
		{
			effect.frost = true;
		}
	}
}