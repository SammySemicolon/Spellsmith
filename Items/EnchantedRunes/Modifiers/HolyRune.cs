using System.Collections.Generic;

namespace Spellsmith.Items.EnchantedRunes.Modifiers
{
	public class HolyRune : ModifierRune
	{
		public override void SetStaticDefaults() 
		{
			Tooltip.SetDefault("Holy");
		}
		public override void setup(Effect effect)
		{
			effect.holy = true;
		}
	}
}