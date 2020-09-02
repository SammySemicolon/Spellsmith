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
        public List<Effect> activeEffects = new List<Effect>();
        public List<ActiveEffectData> tickingEffects = new List<ActiveEffectData>();
        List<Effect> removedEffects;
        List<ActiveEffectData> removedData;
        public Item activeBlaster;
        public override void UpdateLifeRegen()
        {
            if (Spellsmith.instance.abilityInterface.CurrentState == null)
            {
                if (player.HeldItem.modItem is SpellBlaster)
                {
                    Spellsmith.instance.ShowAbilityUI();
                }
            }
            else if (!(player.HeldItem.modItem is SpellBlaster))
            {
                Spellsmith.instance.HideAbilityUI();
            }

            removedEffects = new List<Effect>();
            removedData = new List<ActiveEffectData>();
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
                                    effect.data.Charge(player);
                                    if (!player.channel)
                                    {
                                        RunSpell(effect, velocity);
                                    }
                                    break;
                                }
                            case SpellStyle.Charge:
                                {
                                    bool success = effect.data.Charge(player);
                                    if (success || !player.channel)
                                    {
                                        RunSpell(effect, velocity);
                                    }
                                    break;
                                }
                            case SpellStyle.ChargeRelease:
                                {
                                    effect.data.Charge(player);
                                    if (!player.channel)
                                    {
                                        RunSpell(effect, velocity);
                                    }
                                    break;
                                }
                        }
                    }
                    else
                    {
                        removedEffects.Add(effect);
                    }
                }
            }
            foreach (ActiveEffectData tickingEffect in tickingEffects)
            {
                bool success = tickingEffect.ReduceCooldown();
                if (success)
                {
                    removedEffects.Add(tickingEffect.effect);
                    removedData.Add(tickingEffect);
                }
            }
            foreach (Effect effectToRemove in removedEffects)
            {
                if (activeEffects.Contains(effectToRemove))
                {
                    activeEffects.Remove(effectToRemove);
                }
            }
            foreach (ActiveEffectData dataToRemove in removedData)
            {
                if (tickingEffects.Contains(dataToRemove))
                {
                    tickingEffects.Remove(dataToRemove);
                }
            }
        }
        public void RunSpell(Effect effect, Vector2 velocity)
        {
            effect.RunSpell(player, activeBlaster, velocity, activeBlaster.shootSpeed, activeBlaster.damage, activeBlaster.knockBack);
            if (effect.getTotalCooldown() != 0)
            {
                tickingEffects.Add(effect.data);
            }
            removedEffects.Add(effect);
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