using System.Collections.Generic;
using Terraria.ModLoader;

namespace Spellsmith.Items.EnchantedRunes.Modifiers
{
	public class FieryRune : ModifierRune
	{
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			AddModifierDescriptionTooltip(tooltips, "Ignites the effect");
			AddManaCostIncreaseTooltip(tooltips, 20);
			base.ModifyTooltips(tooltips);
		}
		public override void setup(Effect effect)
		{
			effect.fiery = true;
			effect.manaCostMultiplier *= 1.2f;
		}
	}
}