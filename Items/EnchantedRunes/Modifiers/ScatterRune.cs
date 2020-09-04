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
		public override void setup(SpellEffect effect)
		{
			effect.modifiers.scatterModifier++;
			effect.modifiers.damageMultiplier *= 0.8f;
			effect.modifiers.manaCostMultiplier *= 1.8f;
		}
    }
}