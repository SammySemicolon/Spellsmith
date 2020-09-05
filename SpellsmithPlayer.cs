using Microsoft.Xna.Framework;
using Spellsmith.Items;
using Spellsmith.Items.EnchantedRunes;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using static Spellsmith.Items.EnchantedRunes.SpellEffect;

namespace Spellsmith
{
    public class SpellsmithPlayer : ModPlayer
    {
        public int selectedSpell; 
        public List<SpellEffect> activeEffects = new List<SpellEffect>();
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

            if (activeBlaster != null)
            {
                for (int i = 0; i < activeEffects.Count; i++)
                {
                    if (i < activeEffects.Count)
                    {
                        SpellEffect effect = activeEffects[i];
                        if (effect.OnCooldown)
                        {
                            bool success = effect.ReduceCooldown();
                            if (success)
                            {
                                activeEffects.RemoveAt(i);
                            }
                        }
                        else if (effect.CanRunSpell(player, activeBlaster))
                        {
                            Vector2 velocity = Vector2.Normalize(Main.MouseWorld - player.Center) * activeBlaster.shootSpeed;
                            Vector2 castPosition = player.Center + (Vector2.Normalize(Main.MouseWorld - player.Center) * (activeBlaster.width + 12));

                            switch (effect.Style)
                            {
                                case SpellStyle.Default:
                                    {
                                        RunSpell(effect, velocity, castPosition);
                                        activeEffects.RemoveAt(i);
                                        break;
                                    }
                                case SpellStyle.HoldRelease:
                                    {
                                        effect.Charge(player);
                                        effect.DoChargeVisuals(player, castPosition, activeBlaster);
                                        if (!player.channel)
                                        {
                                            RunSpell(effect, velocity, castPosition);
                                        }
                                        break;
                                    }
                                case SpellStyle.Charge:
                                    {
                                        effect.DoChargeVisuals(player, castPosition, activeBlaster);
                                        bool success = effect.Charge(player);
                                        if (success || !player.channel)
                                        {
                                            RunSpell(effect, velocity, castPosition);
                                        }
                                        break;
                                    }
                                case SpellStyle.ChargeRelease:
                                    {
                                        effect.DoChargeVisuals(player, castPosition, activeBlaster);
                                        effect.Charge(player);
                                        if (!player.channel)
                                        {
                                            RunSpell(effect, velocity, castPosition);
                                        }
                                        break;
                                    }
                            }
                        }
                    }
                }
            }
        }
        public void RunSpell(SpellEffect effect, Vector2 velocity, Vector2 position)
        {
            effect.RunSpell(player, position, activeBlaster, velocity, activeBlaster.shootSpeed, activeBlaster.damage, activeBlaster.knockBack);
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