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
		public override void setup(SpellEffect effect)
		{
			effect.modifiers.fieryModifier = true;
			effect.modifiers.manaCostMultiplier *= 1.2f;
		}
	}
}