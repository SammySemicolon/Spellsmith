using Microsoft.Xna.Framework;
using Spellsmith.Projectiles.RuneProjectiles;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellsmith.Items.EnchantedRunes.Effects
{
	public class VulcanShotRune : EffectRune
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enchanted Rune: Vulcan Shotgun");
		}
		public override SpellEffect effect => new VulcanShotEffect();
	}
	public class VulcanShotEffect : SpellEffect
	{
		public VulcanShotEffect()
		{
			setup();
			manaCost = 60;
			maxCooldown = 300;
			maxCharge = 120;
			tooltip = "Shoots 6 vulcan bullets\nCharge to widen the spread";
		}
		public override SpellType Type => SpellType.Ranged;
        public override SpellStyle Style => SpellStyle.ChargeRelease;
        public override void DoEffect(Player player, Vector2 position, Item item, Vector2 originalVelocity, float shootSpeed, int damage, float knockBack, int importantRun = 0, int totalImportantRuns = 0)
		{
			float charge = getChargePercentage();
			float finalShootSpeed = MathHelper.Max(2, 5 * charge);
			for (int i = 0; i < 6; i++)
			{
				float rot = 90 - (85 * charge);
				Vector2 velocity = originalVelocity.RotatedByRandom(MathHelper.ToRadians(rot));
				ShootProjectile(player, position, velocity, finalShootSpeed, damage / 6, knockBack, ModContent.ProjectileType<VulcanShot>(), importantRun, totalImportantRuns);
			}
		}
        public override void DoChargeVisuals(Player player, Vector2 position, Item item)
		{
			float charge = getChargePercentage();
			if (charge != 1)
			{
				int dustCount = 6;
				for (int i = 0; i < dustCount; i++)
				{
					float radius = 50 - (45 * charge);
					float rot = (player.Center - position).ToRotation();
					float angle = MathHelper.Lerp(0, 6.28f, (float)i / dustCount) + 3.14f * charge;
					Vector2 particleOffset = Vector2.One.RotatedBy(angle) * radius;
					float x = particleOffset.X + (float)Math.Sin(rot);
					float y = particleOffset.Y + (float)Math.Cos(rot);
					Vector2 particlePosition = position + new Vector2(x, y);
					int boltDust = GetDust(0, Main.rand.NextBool() ? 228 : 269, Element);
					Dust dust = Main.dust[Dust.NewDust(particlePosition, 0, 0, boltDust, 0, 0, 0, default, Main.rand.NextFloat(1.2f, 1.6f))];
					dust.noGravity = true;
					dust.velocity = Vector2.Normalize(particlePosition - position) * -(2 - charge);
				}
			}
			else
			{
				int boltDust = GetDust(0, Main.rand.NextBool() ? 228 : 269, Element);
				Dust dust = Main.dust[Dust.NewDust(position, 0, 0, boltDust, 0, 0, 0, default, Main.rand.NextFloat(2f, 2.4f))];
				if (Main.rand.NextBool())
				{
					dust.velocity = new Vector2(Main.rand.NextFloat(-0.2f,0.2f), Main.rand.NextFloat(-1.5f, -0.5f));
				}
				else
				{
					dust.noGravity = true;
				}
			}
		}
        public override void ModifyProjectile(Player player, Projectile projectile, Vector2 executePosition = default)
        {
			projectile.scale = Math.Max(0.4f, projectile.scale * getChargePercentage());
			projectile.timeLeft += Main.rand.Next(0, 621);
			projectile.penetrate += (int)(10 * getChargePercentage());
			base.ModifyProjectile(player, projectile, executePosition);
        }
    }
}