using System.Collections.Generic;
using Terraria.ModLoader;

namespace Spellsmith.Items.EnchantedRunes.Modifiers
{
	public class ScatterRune : ModifierRune
	{
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			AddModifierDescriptionTooltip(tooltips, "The effect happens an additional time");
			AddDamageIncreaseTooltip(tooltips, -20);
			AddManaCostIncreaseTooltip(tooltips, 80);
			base.ModifyTooltips(tooltips);
		}
		public override void setup(Effect effect)
		{
			effect.scatter++;
			effect.damageMultiplier *= 0.8f;
			effect.manaCostMultiplier *= 1.8f;
		}
    }
}