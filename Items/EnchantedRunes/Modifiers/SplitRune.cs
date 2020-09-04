using System.Collections.Generic;
using Terraria.ModLoader;

namespace Spellsmith.Items.EnchantedRunes.Modifiers
{
	public class SplitRune : ModifierRune
	{
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			AddModifierDescriptionTooltip(tooltips, "Splits the effect into 2 smaller ones");
			AddDamageIncreaseTooltip(tooltips, -20);
			AddManaCostIncreaseTooltip(tooltips, 20);
			AddAccuracyIncreaseTooltip(tooltips, 10);
			base.ModifyTooltips(tooltips);
		}
		public override void setup(SpellEffect effect)
		{
			effect.modifiers.splitModifier++;
			effect.modifiers.damageMultiplier *= 0.8f;
			effect.modifiers.accuracy *= 0.9f;
			effect.modifiers.manaCostMultiplier *= 1.2f;
		}
	}
}