using System.Collections.Generic;
using Terraria.ModLoader;

namespace Spellsmith.Items.EnchantedRunes.Modifiers
{
	public class FlurryRune : ModifierRune
	{
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			AddModifierDescriptionTooltip(tooltips, "Grants a chance for the effect to happen an additional time");
			AddDamageIncreaseTooltip(tooltips, -10);
			AddManaCostIncreaseTooltip(tooltips, 50);
			base.ModifyTooltips(tooltips);
		}
		public override void setup(Effect effect)
		{
			effect.flurry++;
			effect.damageMultiplier *= 0.9f;
			effect.manaCostMultiplier *= 1.5f;
		}
	}
}