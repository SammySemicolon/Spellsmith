

using Microsoft.Xna.Framework;
using Spellsmith.Projectiles.RuneProjectiles;
using System;
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
        public virtual Effect effect => null;
    }
    public class Effect
    {
        public enum SpellStyle
        {
            Default,
            ChargeRelease,
            Charge,
            HoldRelease
        }
        public enum SpellType
        {
            None,
            Melee,
            Ranged,
            Magic,
            Summon,
            Utility
        }
        public virtual SpellStyle Style => SpellStyle.Default;
        public virtual SpellType Type => SpellType.None;

        //tooltip stuff
        public string name = "Effect Name goes here";
        public string tooltip = "Rune Description goes here";

        public int bundledEffects = 0;
        //global values
        public bool active = true;
        public float chargeProgress = 0;
        public float maxCharge = 0;

        public float maxCooldown = 0;
        public float cooldown = 0;
        public int manaCost = 0;

        public float cooldownMultipier = 1f;
        public float manaCostMultiplier = 1f;
        public float damageMultiplier = 1f;
        public float accuracy = 1f;
        public float speedMultiplier = 1f;
        public float radiusMultiplier = 1f;

        //stacking modifiers
        public int scatter = 0;
        public int flurry = 0;
        public int split = 0;

        //non stacking modifiers
        public bool fiery = false;
        public bool frost = false;
        public bool holy = false;
        public bool corrupt = false;
        public bool crimson = false;

        public void RunSpell(Player player, Item item, Vector2 originalVelocity, float shootSpeed, int damage, float knockBack)
        {
            int totalImportantRuns = getTotalEffectImportantRuns();
            if (totalImportantRuns >= 1)
            {
                for (int i = 0; i <= totalImportantRuns; i++)
                {
                    DoEffect(player, item, originalVelocity, shootSpeed, damage, knockBack, i, totalImportantRuns);
                }
            }
            else
            {
                DoEffect(player, item, originalVelocity, shootSpeed, damage, knockBack, 0, 0);
            }
            if (Style == SpellStyle.Default || Style == SpellStyle.HoldRelease)
            {
                player.statMana -= getTotalManaCost(player);
            }
            cooldown = maxCooldown;
        }
  
        public virtual bool CanRunSpell(Player player, Item item)
        {
            if (!active)
            {
                return false;
            }
            switch (Type)
            {
                case SpellType.Melee:
                    {
                        item.melee = true;
                        item.ranged = false;
                        item.magic = false;
                        item.summon = false;
                        break;
                    }
                case SpellType.Ranged:
                    {
                        item.melee = false;
                        item.ranged = true;
                        item.magic = false;
                        item.summon = false;
                        break;
                    }
                case SpellType.Magic:
                    {
                        item.melee = false;
                        item.ranged = false;
                        item.magic = true;
                        item.summon = false;
                        break;
                    }
                case SpellType.Summon:
                    {
                        item.melee = false;
                        item.ranged = false;
                        item.magic = false;
                        item.summon = true;
                        break;
                    }
                case SpellType.Utility:
                    {
                        item.melee = false;
                        item.ranged = false;
                        item.magic = false;
                        item.summon = false;
                        break;
                    }
            }
            return cooldown == 0 && player.statMana >= getTotalManaCost(player);
        }
        public virtual bool Charge(Player player, Item item)
        {
            player.itemTime++;
            player.itemAnimation++;
            if (chargeProgress % 10 == 0)
            {
                if (Style == SpellStyle.Charge || Style == SpellStyle.ChargeRelease)
                {
                    player.statMana -= (int)(getTotalManaCost(player) * 10 / maxCharge);
                }
            }
            chargeProgress++;
            return chargeProgress >= maxCharge;
        }
        public virtual bool ReduceCooldown(Player player, Item item)
        {
            if (cooldown > 0)
            {
                cooldown--;
            }
            return cooldown == 0;
        }
        public virtual void DoEffect(Player player, Item item, Vector2 originalVelocity, float shootSpeed, int damage, float knockBack, int importantRun = 0, int totalImportantRuns = 0)
        {
        }

        public virtual void ModifyProjectile(Player player, Projectile projectile, Vector2 executePosition = new Vector2())
        {
            Main.NewText(executePosition);
            if (split > 0)
            {
                projectile.scale *= (float)Math.Pow(0.9, split + 1);
            }
            if (projectile.modProjectile is SpellProjectile)
            {
                SpellProjectile spellProjectile = (SpellProjectile)projectile.modProjectile;
                spellProjectile.casterEffect = this;
            }
        }

        public virtual void ShootProjectile(Player player, Vector2 originalVelocity, float speed, int damage, float knockBack, int projectileID, int importantRun, int totalImportantRuns)
        {
            float importantSpread = totalImportantRuns == 0 ? 0 : getImportantSpread(importantRun, totalImportantRuns, 8);
            int totalRuns = getTotalRuns();
            float spread = MathHelper.ToRadians(getSpread(totalRuns, 8));
            for (int j = 0; j < totalRuns; j++)
            {
                Vector2 perturbedSpeed = getImportantShootVelocity(originalVelocity, speed, spread, importantSpread);
                int projectile = Projectile.NewProjectile(player.Center.X, player.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, projectileID, getTotalDamage(damage), getTotalKnockBack(knockBack), player.whoAmI);
                ModifyProjectile(player, Main.projectile[projectile]);
            }
        }
        public virtual void ShootSkyProjectile(Player player, Item item, float YOffset, float speed, int damage, float knockBack, int projectileID, int importantRun = 0, int totalImportantRuns = 0)
        {
            float importantSpread = totalImportantRuns == 0 ? 0 : getImportantSpread(importantRun, totalImportantRuns, 40); //evenly offset spawn pos
            int totalRuns = getTotalRuns();
            float spread = getSpread(totalRuns, 8); //randomly offset target pos
            for (int j = 0; j < totalRuns; j++)
            {
                Vector2 targetPos = Main.MouseWorld + getInnacurateVector(spread * 8);
                Vector2 offset = getImportantShootVelocity(new Vector2(0, -1), YOffset, MathHelper.ToRadians(spread), importantSpread);
                Vector2 spawnPos = player.Center + offset;
                Vector2 velocity = Vector2.Normalize(targetPos - spawnPos) * speed;
                int projectile = Projectile.NewProjectile(spawnPos.X, spawnPos.Y, velocity.X, velocity.Y, projectileID, getTotalDamage(damage), getTotalKnockBack(knockBack), player.whoAmI);
                ModifyProjectile(player, Main.projectile[projectile], targetPos);
            }
        }
        #region TONS of getters
        public virtual Vector2 getInnacurateVector(float spread)
        {
            return new Vector2(Main.rand.NextFloat(-spread, spread), Main.rand.NextFloat(-spread, spread));
        }
        public virtual float getTotalKnockBack(float originalKnockBack)
        {
            return originalKnockBack * damageMultiplier;
        }
        public virtual int getTotalDamage(int originalDamage)
        {
            return (int)Math.Max(1, originalDamage * damageMultiplier);
        }

        public virtual int getTotalManaCost(Player player)
        {
            double cost = (manaCost * manaCostMultiplier) * player.manaCost;
            if (bundledEffects > 1)
            {
                cost *= Math.Pow(1.1f, bundledEffects);
            }
            return (int)cost;
        }

        public virtual int getTotalCooldown()
        {
            return (int)(maxCooldown * cooldownMultipier);
        }
        public virtual int getTotalEffectImportantRuns()
        {
            return split;
        }
        public virtual int getTotalRuns()
        {
            int count = 1 + scatter;
            if (flurry != 0)
            {
                count += Main.rand.Next(0, flurry + 1);
            }
            return count;
        }

        public virtual float getSpread(int runs, float baseSpread)
        {
            float finalSpread = 3;
            if (runs > 1)
            {
                finalSpread = baseSpread * runs;
            }
            return finalSpread * accuracy;
        }

        public virtual float getImportantSpread(int importantRun, int importantRuns, float spread)
        {
            float baseRotation = MathHelper.ToRadians(spread);

            return MathHelper.Lerp(-baseRotation, baseRotation, (float)importantRun / importantRuns);
        }

        public virtual Vector2 getShootVelocity(Vector2 originalVelocity, float speed)
        {
            return Vector2.Normalize(originalVelocity) * speed;
        }
        public virtual Vector2 getImportantShootVelocity(Vector2 originalVelocity, float speed, float spread, float importantSpread)
        {
            Vector2 perturbedSpeed = getShootVelocity(originalVelocity, speed).RotatedBy(importantSpread).RotatedByRandom(spread);
            float scale = 1f - (Main.rand.NextFloat() * (0.3f * (float)Math.Pow(accuracy,3)));
            perturbedSpeed *= scale;
            return perturbedSpeed;
        }
        #endregion
    }
}