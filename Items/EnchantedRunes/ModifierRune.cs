using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Spellsmith.Items.EnchantedRunes
{
    public abstract class ModifierRune : BaseRune
    {
        public void AddModifierDescriptionTooltip(List<TooltipLine> tooltips, string description)
        {
            tooltips.Add(new TooltipLine(Spellsmith.instance, "ModifierTooltip", description));
        }
        public void AddManaCostIncreaseTooltip(List<TooltipLine> tooltips, int percentage)
        {
            string manaCost = "Effect uses " + Math.Abs(percentage) + (percentage > 0 ? "% more mana" : "% less mana");
            tooltips.Add(new TooltipLine(Spellsmith.instance, "ModifierManaCost", manaCost));
        }
        public void AddCooldownIncreaseTooltip(List<TooltipLine> tooltips, int percentage)
        {
            string cooldown = "Effect cooldown is " + Math.Abs(percentage) + (percentage > 0 ? "% longer" : "% shorter");
            tooltips.Add(new TooltipLine(Spellsmith.instance, "ModifierCooldown", cooldown));
        }
        public void AddDamageIncreaseTooltip(List<TooltipLine> tooltips, int percentage)
        {
            string damage = "Effect deals " + Math.Abs(percentage) + (percentage > 0 ? "% more damage" : "% less damage");
            tooltips.Add(new TooltipLine(Spellsmith.instance, "ModifierDamage", damage));
        }
        public void AddAccuracyIncreaseTooltip(List<TooltipLine> tooltips, int percentage)
        {
            string accuracy = "Effect is " + Math.Abs(percentage) + (percentage > 0 ? "% more accurate" : "% less accurate");
            tooltips.Add(new TooltipLine(Spellsmith.instance, "ModifierAccuracy", accuracy));
        }
        public DefaultFuckUpEffect fuckUpEffect => new DefaultFuckUpEffect();
    }
    public class DefaultFuckUpEffect : Effect
    {
        public override void DoEffect(Player player, Item item, Vector2 originalVelocity, float shootSpeed, int damage, float knockBack, int importantRun = 0, int totalImportantRuns = -10)
        {
            if (player is Player)
            {
                string deathMessage = player.name + " fucked up their spell";
               /* if (fieryModifier)
                {
                    deathMessage = player.name + " erupted into flames";
                }
                if (frostModifier)
                {
                    deathMessage = player.name + " became a cube of ice";
                }
                if (holyModifier)
                {
                    deathMessage = player.name + " turned into rainbow dust";
                }
                if (crimsonModifier)
                {
                    deathMessage = player.name + " became a clump of flesh";
                }
                if (corruptModifier)
                {
                    deathMessage = player.name + " sacrificed themselves to the dark gods";
                }*/ //redo this one day
                player.KillMe(PlayerDeathReason.ByCustomReason(deathMessage), player.statLifeMax2, 0);
            }
        }
    }
}