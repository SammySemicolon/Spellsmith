using Microsoft.Xna.Framework;
using Spellsmith.Projectiles.RuneProjectiles;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellsmith.Items.EnchantedRunes.Effects
{
	public class GraniteSurgeRune : EffectRune
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enchanted Rune: Surge Bolt");
		}
		public override SpellEffect effect => new GraniteSurgeEffect();
	}
	public class GraniteSurgeEffect : SpellEffect
	{
		public GraniteSurgeEffect()
		{
			setup();
			manaCost = 30;
			maxCharge = 40;
			tooltip = "Creates a piercing bolt of condensed granite energy";
		}
		public override SpellType Type => SpellType.Magic;
        public override SpellStyle Style => SpellStyle.Charge;
        public override void DoEffect(Player player, Vector2 position, Item item, Vector2 originalVelocity, float shootSpeed, int damage, float knockBack, int importantRun = 0, int totalImportantRuns = 0)
		{
			float charge = getChargePercentage();
			float finalShootSpeed = MathHelper.Max(3, 5 * charge);
			ShootProjectile(player, position, originalVelocity, finalShootSpeed, damage, knockBack, ModContent.ProjectileType<GraniteSurge>(), importantRun, totalImportantRuns);
		}
        public override void DoChargeVisuals(Player player, Vector2 position, Item item)
		{
			float charge = getChargePercentage();
			for (int i = 0; i < 2; i++)
			{
				float radius = 40 - (40 * charge);
				float rot = (player.Center - position).ToRotation();
				float angle = 0;
				if (i == 0)
                {
					angle += 3.14f;
                }
				Vector2 particleOffset = Vector2.One.RotatedBy(angle + (6.28f * charge)) * radius;
				float x = particleOffset.X + (float)Math.Sin(rot);
				float y = particleOffset.Y + (float)Math.Cos(rot);
				Vector2 particlePosition = position + new Vector2(x,y);
				int boltDust = GetDust(0, Main.rand.NextBool() ? 45 : 15, Element);
				Dust dust = Main.dust[Dust.NewDust(particlePosition, 0, 0, boltDust, 0, 0, 0, default, Main.rand.NextFloat(1.2f, 1.6f))];
				dust.noGravity = true;
				dust.velocity = Vector2.Normalize(particlePosition - position) * -(5 - 4 * charge);
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