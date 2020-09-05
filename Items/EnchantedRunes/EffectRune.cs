using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace Spellsmith.Items.EnchantedRunes
{
    public abstract class EffectRune : BaseRune
    {
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Spellsmith.instance, "EffectTooltip", effect.tooltip));
            string manaCostTooltip = effect.getTotalManaCost(Main.LocalPlayer) == 0 ? "Uses no mana" : "Uses " + effect.getTotalManaCost(Main.LocalPlayer) + " mana";
            tooltips.Add(new TooltipLine(Spellsmith.instance, "EffectManaCost", manaCostTooltip));
            string cooldown = effect.getTotalCooldown() == 0 ? "No Cooldown" : effect.getTotalCooldown() / 60 + " Second cooldown";
            tooltips.Add(new TooltipLine(Spellsmith.instance, "EffectCooldown", cooldown));
            base.ModifyTooltips(tooltips);
        }
        public virtual SpellEffect effect => null;
    }
}