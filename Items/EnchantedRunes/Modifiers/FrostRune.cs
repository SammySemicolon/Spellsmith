using System.Collections.Generic;
using Terraria.ModLoader;

namespace Spellsmith.Items.EnchantedRunes.Modifiers
{
	public class FrostRune : ModifierRune
	{
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			AddModifierDescriptionTooltip(tooltips, "Chills the effect");
			AddManaCostIncreaseTooltip(tooltips, 40);
			base.ModifyTooltips(tooltips);
		}
		public override void setup(SpellEffect effect)
		{
			effect.modifiers.frostModifier = true;
			effect.modifiers.manaCostMultiplier *= 1.4f;
		}
	}
}