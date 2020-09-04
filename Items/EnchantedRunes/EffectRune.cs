

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
        public virtual SpellEffect effect => null;
    }
    public class SpellEffect
    {
        public SpellEffect()
        {
            setup();
        }
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
        public enum SpellElement
        {
            None,
            Fiery,
            Frost,
            Holy,
            Corrupt,
            Crimson
        }
        public virtual SpellStyle Style => SpellStyle.Default;
        public virtual SpellType Type => SpellType.None;
        public SpellElement Element = SpellElement.None;

        //tooltip stuff
        public string name = "Effect Name goes here";
        public string tooltip = "Rune Description goes here";

        public int bundledEffects = 0;
        public float maxCharge = 0;
        public float maxCooldown = 0;
        public int manaCost = 0;

        public EffectModifiers modifiers;
        public ActiveEffectData data;

        public virtual void setup()
        {
            modifiers = new EffectModifiers(this);
            data = new ActiveEffectData(this);
        }

        public virtual void SortOutClass(Item item)
        {
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
        }
        public virtual bool CanRunSpell(Player player, Item item)
        {
            if (Style == SpellStyle.Default || Style == SpellStyle.HoldRelease)
            {
                return !data.OnCooldown && player.statMana >= getTotalManaCost(player);
            }
            else
            {
                return !data.OnCooldown && player.statMana >= getTotalManaCost(player, getChargePercentage());
            }
        }
        public void RunSpell(Player player, Vector2 position, Item item, Vector2 originalVelocity, float shootSpeed, int damage, float knockBack)
        {

            int totalImportantRuns = getTotalEffectImportantRuns();
            if (totalImportantRuns >= 1)
            {
                for (int i = 0; i <= totalImportantRuns; i++)
                {
                    DoEffect(player, position, item, originalVelocity, shootSpeed, damage, knockBack, i, totalImportantRuns);
                }
            }
            else
            {
                DoEffect(player, position, item, originalVelocity, shootSpeed, damage, knockBack, 0, 0);
            }
            if (Style == SpellStyle.Default || Style == SpellStyle.HoldRelease)
            {
                player.statMana -= getTotalManaCost(player);
            }
            else
            {
                player.statMana -= getTotalManaCost(player, getChargePercentage());
            }
            data = new ActiveEffectData(this);
            data.MaxOutCooldown();
            Element = SpellElement.None;
        }
        public virtual void DoEffect(Player player, Vector2 position, Item item, Vector2 originalVelocity, float shootSpeed, int damage, float knockBack, int importantRun = 0, int totalImportantRuns = 0)
        {
        }

        public virtual void ModifyProjectile(Player player, Projectile projectile, Vector2 executePosition = new Vector2())
        {
            if (modifiers.splitModifier > 0)
            {
                projectile.scale *= (float)Math.Pow(0.9, modifiers.splitModifier + 1);
            }
            if (projectile.modProjectile is SpellProjectile)
            {
                SpellProjectile spellProjectile = (SpellProjectile)projectile.modProjectile;
                ChooseElement(spellProjectile);
                projectile.netUpdate = true;
            }
        }
        public virtual void DoChargeVisuals(Player player, Vector2 position, Item item)
        {

        }
        public void ChooseElement(SpellProjectile spellProjectile)
        {
            spellProjectile.casterEffect = this;

            List<SpellElement> elements = new List<SpellElement>();
            if (modifiers.fieryModifier)
            {
                elements.Add(SpellElement.Fiery);
            }
            if (modifiers.frostModifier)
            {
                elements.Add(SpellElement.Frost);
            }
            if (modifiers.holyModifier)
            {
                elements.Add(SpellElement.Holy);
            }
            if (modifiers.crimsonModifier)
            {
                elements.Add(SpellElement.Crimson);
            }
            if (modifiers.corruptModifier)
            {
                elements.Add(SpellElement.Corrupt);
            }
            if (elements.Count > 0)
            {
                spellProjectile.Element = elements[Main.rand.Next(elements.Count)];
                Element = spellProjectile.Element;

                return;
            }
            Element = SpellElement.None;
            spellProjectile.Element = SpellElement.None;
        }
        public virtual void ShootProjectile(Player player, Vector2 position, Vector2 originalVelocity, float speed, int damage, float knockBack, int projectileID, int importantRun, int totalImportantRuns)
        {
            float importantSpread = totalImportantRuns == 0 ? 0 : getImportantSpread(importantRun, totalImportantRuns, 8);
            int totalRuns = getTotalRuns();
            float spread = MathHelper.ToRadians(getSpread(totalRuns, 8));
            for (int j = 0; j < totalRuns; j++)
            {
                Vector2 perturbedSpeed = getImportantShootVelocity(originalVelocity, speed, spread, importantSpread);
                int projectile = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, projectileID, getTotalDamage(damage), getTotalKnockBack(knockBack), player.whoAmI);
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
            return originalKnockBack * modifiers.knockbackMultiplier;
        }
        public virtual int getTotalDamage(int originalDamage)
        {
            return (int)Math.Max(1, originalDamage * modifiers.damageMultiplier);
        }

        public virtual float getChargePercentage()
        {
            return data.chargeProgress / maxCharge;
        }
        public virtual int getTotalManaCost(Player player, float charge = 1f)
        {
            double cost = (manaCost * modifiers.manaCostMultiplier) * player.manaCost;
            if (bundledEffects > 1)
            {
                cost *= Math.Pow(1.1f, bundledEffects);
            }
            return (int)(cost * charge);
        }

        public virtual int getTotalCooldown()
        {
            return (int)(maxCooldown * modifiers.cooldownMultipier);
        }
        public virtual int getTotalEffectImportantRuns()
        {
            return modifiers.splitModifier;
        }
        public virtual int getTotalRuns()
        {
            int count = 1 + modifiers.scatterModifier;
            if (modifiers.flurryModifier != 0)
            {
                count += Main.rand.Next(0, modifiers.flurryModifier + 1);
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
            return finalSpread * modifiers.accuracy;
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
            float scale = 1f - (Main.rand.NextFloat() * (0.3f * (float)Math.Pow(modifiers.accuracy, 3)));
            perturbedSpeed *= scale;
            return perturbedSpeed;
        }


        public static int[] fireDusts = new int[] { 6, 127, 158 };
        public static int[] frostDusts = new int[] { 135, 39, 132 };
        public static int[] holyDusts = new int[] { 199, 49, 188 };
        public static int[] corruptDusts = new int[] { 75, 61, 74 };
        public static int[] crimsonDusts = new int[] { 170, 125, 130 };

        public static Color SetupColor(Color defaultColor, SpellElement Element)
        {
            switch (Element)
            {
                case (SpellElement.None):
                    {
                        return defaultColor;
                    }
                case (SpellElement.Fiery):
                    {
                        return new Color(255, 142, 47);
                    }
                case (SpellElement.Frost):
                    {
                        return new Color(21, 144, 222);
                    }
                case (SpellElement.Holy):
                    {
                        return new Color(236, 83, 179);
                    }
                case (SpellElement.Corrupt):
                    {
                        return new Color(181, 255, 56);
                    }

                case (SpellElement.Crimson):
                    {
                        return new Color(255, 47, 47);
                    }
            }
            return defaultColor;
        }
        public static int GetDust(int value, int defaultDust, SpellElement Element)
        {
            switch (Element)
            {
                case (SpellElement.None):
                    {
                        return defaultDust;
                    }
                case (SpellElement.Fiery):
                    {
                        return fireDusts[value];
                    }
                case (SpellElement.Frost):
                    {
                        return frostDusts[value];
                    }
                case (SpellElement.Holy):
                    {
                        return holyDusts[value];
                    }
                case (SpellElement.Corrupt):
                    {
                        return corruptDusts[value];
                    }
                case (SpellElement.Crimson):
                    {
                        return crimsonDusts[value];
                    }
            }
            return 0;
        }
        #endregion
    }
    public class EffectModifiers
    {
        public EffectModifiers(SpellEffect effect)
        {
            this.effect = effect;
        }
        public SpellEffect effect;
        //stacking modifiers
        public int scatterModifier = 0;
        public int flurryModifier = 0;
        public int splitModifier = 0;

        //non stacking modifiers
        public bool fieryModifier = false;
        public bool frostModifier = false;
        public bool holyModifier = false;
        public bool corruptModifier = false;
        public bool crimsonModifier = false;

        public float cooldownMultipier = 1f;
        public float manaCostMultiplier = 1f;
        public float damageMultiplier = 1f;
        public float knockbackMultiplier = 1f;
        public float accuracy = 1f;
        public float speedMultiplier = 1f;
        public float radiusMultiplier = 1f;
    }
    public class ActiveEffectData
    {
        public ActiveEffectData(SpellEffect effect)
        {
            this.effect = effect;
        }
        public SpellEffect effect;

        public float chargeProgress = 0;
        public float cooldown = 0;

        public bool OnCooldown => cooldown > 0;
        public virtual bool Charge(Player player)
        {
            SpellEffect.SpellStyle Style = effect.Style;
            player.itemTime++;
            player.itemAnimation++;
            Vector2 direction = Vector2.Normalize(player.Center - Main.MouseWorld);
            player.direction = Main.MouseWorld.X < player.Center.X ? -1 : 1;
            if (player.direction == 1)
            {
                direction = -direction;
            }
            player.itemRotation = direction.ToRotation();
            if (chargeProgress < effect.maxCharge)
            {
                chargeProgress++;
            }
            if (Style == SpellEffect.SpellStyle.Charge || Style == SpellEffect.SpellStyle.ChargeRelease)
            {
                return chargeProgress >= effect.maxCharge;
            }
            return false;
        }
        public virtual bool ReduceCooldown()
        {
            if (effect.getTotalCooldown() == 0)
            {
                return true;
            }
            if (cooldown > 0)
            {
                cooldown--;
            }
            Main.NewText(cooldown);
            return cooldown == 0;
        }
        public virtual void MaxOutCooldown()
        {
            cooldown = effect.getTotalCooldown();
            chargeProgress = 0;
        }
    }
}