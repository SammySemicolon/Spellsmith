using System.Collections.Generic;
using Terraria.ModLoader;

namespace Spellsmith.Items.EnchantedRunes.Modifiers
{
	public class CorruptRune : ModifierRune
	{
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            AddModifierDescriptionTooltip(tooltips, "Corrupts the effect");
            AddManaCostIncreaseTooltip(tooltips, 60);
            base.ModifyTooltips(tooltips);
        }
        public override void setup(Effect effect)
		{
			effect.modifiers.corruptModifier = true;
			effect.modifiers.manaCostMultiplier *= 1.6f;
		}
    }
}