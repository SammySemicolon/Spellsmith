using Microsoft.Xna.Framework;
using Spellsmith.Projectiles.RuneProjectiles;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellsmith.Items.EnchantedRunes.Effects
{
	public class HexRune : EffectRune
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enchanted Rune: Hex bolt");
		}
		public override SpellEffect effect => new HexEffect();
	}
	public class HexEffect : SpellEffect
	{
		public HexEffect()
		{
			setup();
			manaCost = 12;
			maxCharge = 60;
			tooltip = "Creates a homing hex bolt, which bounces between enemies";
		}
		public override SpellType Type => SpellType.Magic;
        public override SpellStyle Style => SpellStyle.Charge;
        public override void DoEffect(Player player, Vector2 position, Item item, Vector2 originalVelocity, float shootSpeed, int damage, float knockBack, int importantRun = 0, int totalImportantRuns = 0)
		{
			float charge = getChargePercentage();
			float finalShootSpeed = MathHelper.Max(2, 4 * charge);
			ShootProjectile(player, position, originalVelocity, finalShootSpeed, damage, knockBack, ModContent.ProjectileType<HexBolt>(), importantRun, totalImportantRuns);
		}
		public override void DoChargeVisuals(Player player, Vector2 position, Item item)
		{
			int hexDust = GetDust(0, 27, Element);
			int orbitDust = GetDust(1, 159, Element);
			float charge = getChargePercentage();
			for (int i = 0; i < 2; i++)
			{
				float radius = 50 - (40 * charge);
				Vector2 particlePosition = position + Vector2.One.RotatedBy(Main.rand.NextFloat(0, 6.28f)) * radius;

				int dustID = hexDust;
				if (i == 0)
				{
					dustID = orbitDust;
				}
				Dust dust = Main.dust[Dust.NewDust(particlePosition, 0, 0, dustID, 0, 0, 0, default, Main.rand.NextFloat(1.2f, 1.6f))];
				dust.noGravity = true;
				dust.velocity = Vector2.Normalize(particlePosition - position) * -(5 - 4 * charge);
			}
		}
        public override void ModifyProjectile(Player player, Projectile projectile, Vector2 executePosition = default)
        {
			projectile.scale = Math.Max(0.4f, projectile.scale * getChargePercentage());
			projectile.timeLeft += Main.rand.Next(0, 621);
			(projectile.modProjectile as HexBolt).projectile.ai[1] = MathHelper.Max(2, 4 * getChargePercentage());
			base.ModifyProjectile(player, projectile, executePosition);
        }
    }
}