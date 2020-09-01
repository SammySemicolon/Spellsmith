namespace Spellsmith.Items.EnchantedRunes.Modifiers
{
	public class CorruptRune : ModifierRune
	{
		public override void SetStaticDefaults() 
		{
			Tooltip.SetDefault("Corrupt");
		}
		public override void setup(Effect effect)
		{
			effect.corrupt = true;
		}
    }
}