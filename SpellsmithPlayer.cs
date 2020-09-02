using Microsoft.Xna.Framework;
using Spellsmith.Items;
using Spellsmith.Items.EnchantedRunes;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using static Spellsmith.Items.EnchantedRunes.Effect;

namespace Spellsmith
{
    public class SpellsmithPlayer : ModPlayer
    {
        public int selectedSpell;
        public List<Effect> cooldownEffects = new List<Effect>();
        public List<Effect> activeEffects = new List<Effect>();
        List<Effect> removedEffects = new List<Effect>();
        public Item activeBlaster;
        public override void UpdateLifeRegen()
        {
            removedEffects = new List<Effect>();
            if (activeBlaster != null)
            {
                foreach (Effect effect in activeEffects)
                {
                    if (effect.CanRunSpell(player, activeBlaster))
                    {
                        Vector2 velocity = Vector2.Normalize(Main.MouseWorld - player.Center) * activeBlaster.shootSpeed;
                        switch (effect.Style)
                        {
                            case SpellStyle.Default:
                                {
                                    RunSpell(effect, velocity);
                                    break;
                                }
                            case SpellStyle.HoldRelease:
                                {
                                    effect.Charge(player, activeBlaster);
                                    if (!player.channel)
                                    {
                                        RunSpell(effect, velocity);
                                    }
                                    break;
                                }
                            case SpellStyle.Charge:
                                {
                                    bool success = effect.Charge(player, activeBlaster);
                                    if (success || !player.channel)
                                    {
                                        RunSpell(effect, velocity);
                                    }
                                    break;
                                }
                            case SpellStyle.ChargeRelease:
                                {
                                    effect.Charge(player, activeBlaster);
                                    if (!player.channel)
                                    {
                                        RunSpell(effect, velocity);
                                    }
                                    break;
                                }
                        }
                    }
                }
            }
            foreach (Effect cooldownEffect in cooldownEffects)
            {
                bool success = cooldownEffect.ReduceCooldown(player, activeBlaster);
                if (success)
                {
                    removedEffects.Add(cooldownEffect);
                }
            }
            foreach (Effect cooldownEffect in removedEffects)
            {
                activeEffects.Remove(cooldownEffect);
                cooldownEffects.Remove(cooldownEffect);
            }
        }
        public void RunSpell(Effect effect, Vector2 velocity)
        {
            effect.RunSpell(player, activeBlaster, velocity, activeBlaster.shootSpeed, activeBlaster.damage, activeBlaster.knockBack);
            if (effect.maxCooldown != 0)
            {
                cooldownEffects.Add(effect);
            }
            else
            {
                removedEffects.Add(effect);
            }
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Spellsmith.Spell1.JustPressed)
            {
                selectedSpell = 0;
            }
            if (Spellsmith.Spell2.JustPressed)
            {
                selectedSpell = 1;
            }
            if (Spellsmith.Spell3.JustPressed)
            {
                selectedSpell = 2;
            }
            if (Spellsmith.Spell4.JustPressed)
            {
                selectedSpell = 3;
            }
            if (Spellsmith.NextSpell.JustPressed)
            {
                selectedSpell--;
                if (selectedSpell < 0)
                {
                    selectedSpell = 3;
                }
            }
            if (Spellsmith.PreviousSpell.JustPressed)
            {
                selectedSpell++;
                if (selectedSpell > 3)
                {
                    selectedSpell = 0;
                }
            }
        }
    }
}