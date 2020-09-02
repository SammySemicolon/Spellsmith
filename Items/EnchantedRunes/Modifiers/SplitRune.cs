using System.Collections.Generic;
using Terraria.ModLoader;

namespace Spellsmith.Items.EnchantedRunes.Modifiers
{
	public class SplitRune : ModifierRune
	{
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			AddModifierDescriptionTooltip(tooltips, "Splits the effect into 2 smaller ones");
			AddDamageIncreaseTooltip(tooltips, -30);
			AddManaCostIncreaseTooltip(tooltips, 20);
			AddAccuracyIncreaseTooltip(tooltips, 10);
			base.ModifyTooltips(tooltips);
		}
		public override void setup(Effect effect)
		{
			effect.split++;
			effect.damageMultiplier *= 0.7f;
			effect.accuracy *= 0.9f;
			effect.manaCostMultiplier *= 1.2f;
		}
	}
}