using System.Collections.Generic;

namespace Spellsmith.Items.EnchantedRunes.Modifiers
{
	public class FieryRune : ModifierRune
	{
		public override void SetStaticDefaults() 
		{
			Tooltip.SetDefault("Fiery");
		}
		public override void setup(Effect effect)
		{
			effect.fiery = true;
		}
	}
}