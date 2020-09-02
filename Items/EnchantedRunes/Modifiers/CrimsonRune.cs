using System.Collections.Generic;
using Terraria.ModLoader;

namespace Spellsmith.Items.EnchantedRunes.Modifiers
{
	public class CrimsonRune : ModifierRune
	{
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			AddModifierDescriptionTooltip(tooltips, "Makes the effect bloody");
			AddManaCostIncreaseTooltip(tooltips, 80);
			base.ModifyTooltips(tooltips);
		}
		public override void setup(Effect effect)
		{
			effect.modifiers.crimsonModifier = true;
			effect.modifiers.manaCostMultiplier *= 1.8f;
		}
	}
}