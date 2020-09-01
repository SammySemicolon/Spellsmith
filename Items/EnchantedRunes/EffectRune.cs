

using Microsoft.Xna.Framework;
using Spellsmith.Projectiles.RuneProjectiles;
using System;
using System.Security.Cryptography.X509Certificates;
using Terraria;
using Terraria.ModLoader.IO;

namespace Spellsmith.Items.EnchantedRunes
{
    public class EffectRune : BaseRune
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Effect");
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

        public float speedMultiplier = 1f;
        public float accuracy = 1f;
        public float defaultDamageMultiplier = 1;
        public float damageMultiplier = 1;

        public int scatter = 0;
        public int flurry = 0;
        public int split = 0;

        public bool fiery = false;
        public bool frost = false;
        public bool holy = false;
        public bool corrupt = false;
        public bool crimson = false;

        public void RunSpell(Entity entity, Item item, Vector2 originalVelocity, float shootSpeed, int damage, float knockBack)
        {
            int totalImportantRuns = getTotalEffectImportantRuns();
            if (totalImportantRuns >= 1)
            {
                for (int i = 0; i <= totalImportantRuns; i++)
                {
                    DoEffect(entity, item, originalVelocity, shootSpeed, damage, knockBack, i, totalImportantRuns);
                }
            }
            else
            {
                DoEffect(entity, item, originalVelocity, shootSpeed, damage, knockBack, 0, 0);
            }
        }
        public virtual void DoEffect(Entity entity, Item item, Vector2 originalVelocity, float shootSpeed, int damage, float knockBack, int importantRun = 0, int totalImportantRuns = 0)
        {
        }

        public virtual void ModifyProjectile(Entity entity, Projectile projectile, Vector2 executePosition = new Vector2())
        {
            Main.NewText(executePosition);
            if (split > 0)
            {
                projectile.scale *= (float)Math.Pow(0.95, split + 1);
            }
            if (projectile.modProjectile is SpellProjectile)
            {
                SpellProjectile spellProjectile = (SpellProjectile)projectile.modProjectile;
                spellProjectile.casterEffect = this;
            }
        }

        public virtual void ShootProjectile(Entity entity, Vector2 originalVelocity, float speed, int damage, float knockBack, int projectileID, int importantRun, int totalImportantRuns)
        {
            float importantSpread = totalImportantRuns == 0 ? 0 : getImportantSpread(importantRun, totalImportantRuns, 8);
            int totalRuns = getTotalRuns();
            float spread = MathHelper.ToRadians(getSpread(totalRuns, 8));
            for (int j = 0; j < totalRuns; j++)
            {
                Vector2 perturbedSpeed = getImportantShootVelocity(originalVelocity, speed, spread, importantSpread);
                int projectile = Projectile.NewProjectile(entity.Center.X, entity.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, projectileID, getTotalDamage(damage), getTotalKnockBack(knockBack), entity.whoAmI);
                ModifyProjectile(entity, Main.projectile[projectile]);
            }
        }
        public virtual void ShootSkyProjectile(Entity entity, Item item, float YOffset, float speed, int damage, float knockBack, int projectileID, int importantRun = 0, int totalImportantRuns = 0)
        {
            float importantSpread = totalImportantRuns == 0 ? 0 : getImportantSpread(importantRun, totalImportantRuns, 8); //evenly offset spawn pos
            int totalRuns = getTotalRuns();
            float spread = getSpread(totalRuns, 8); //randomly offset target pos
            for (int j = 0; j < totalRuns; j++)
            {
                Vector2 targetPos = Main.MouseWorld + getInnacurateVector(spread * 8);
                Vector2 offset = getImportantShootVelocity(new Vector2(0, -1), YOffset, MathHelper.ToRadians(spread), importantSpread);
                Vector2 spawnPos = entity.Center + offset;
                Vector2 velocity = Vector2.Normalize(targetPos - spawnPos) * speed;
                int projectile = Projectile.NewProjectile(spawnPos.X, spawnPos.Y, velocity.X, velocity.Y, projectileID, getTotalDamage(damage), getTotalKnockBack(knockBack), entity.whoAmI);
                ModifyProjectile(entity, Main.projectile[projectile], targetPos);
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
            return Math.Max(1, (int)(originalDamage * damageMultiplier));
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

            return MathHelper.Lerp(-baseRotation, baseRotation, importantRun / importantRuns);
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