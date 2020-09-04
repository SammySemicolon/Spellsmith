using System.Collections.Generic;
using Terraria.ModLoader;

namespace Spellsmith.Items.EnchantedRunes.Modifiers
{
	public class HolyRune : ModifierRune
	{
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			AddModifierDescriptionTooltip(tooltips, "Turns the effect hallow");
			AddManaCostIncreaseTooltip(tooltips, 100);
			base.ModifyTooltips(tooltips);
		}
		public override void setup(SpellEffect effect)
		{
			effect.modifiers.holyModifier = true;
			effect.modifiers.manaCostMultiplier *= 2f;
		}
	}
}