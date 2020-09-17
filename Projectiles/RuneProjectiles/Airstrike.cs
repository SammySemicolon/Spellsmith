using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spellsmith.Items.EnchantedRunes;
using System;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace Spellsmith.Projectiles.RuneProjectiles
{
	public class Airstrike : SpellProjectile
	{
		public float Y = 1000000;
		public bool boom = false;
		public Vector2 lastVelocity;
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 40;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			projectile.width = 200;
			projectile.height = 200;
			projectile.timeLeft = 10;
			projectile.extraUpdates = 1;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.scale = 1f;
		}
		public override string Texture => "Spellsmith/Invisible";
		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			spriteBatch.End();
			Main.spriteBatch.Begin(default, BlendState.Additive, default, default, default, default, Main.GameViewMatrix.EffectMatrix);
			Vector2 drawOrigin = new Vector2(projectile.width / 2, projectile.height / 2);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				if (k > projectile.ai[1])
				{
					if (!projectile.oldPos[k].Equals(projectile.position))
					{
						Vector2 glowPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
						Color finalColor = color * ((float)(projectile.oldPos.Length - k) / projectile.oldPos.Length);
						float glowScale = (projectile.scale - projectile.scale * (k / projectile.oldPos.Length)) * 0.5f;
						string texture = "Spellsmith/VFX/LongTrail";
						float rotation = lastVelocity.ToRotation() + 1.57f;
						if (k == 0)
                        {
							texture = "Spellsmith/VFX/Flash";
							rotation = projectile.rotation;
						}
						spriteBatch.Draw(GetTexture(texture), glowPos, null, finalColor, rotation, drawOrigin, glowScale, SpriteEffects.None, 0f);
					}
				}
			}
			Main.spriteBatch.End();
			Main.spriteBatch.Begin();
			base.PostDraw(spriteBatch, lightColor);
		}
		public override bool? CanHitNPC(NPC target)
		{
			return false;
		}
		public override void AI()
		{
			if (projectile.velocity != Vector2.Zero)
            {
				lastVelocity = projectile.velocity;
            }
			projectile.timeLeft++;
			projectile.ai[0] += 0.12f;
			projectile.rotation += 0.06f;
			if (Y < projectile.Center.Y && !boom)
			{
				MyMainGoal();
			}
			if (boom)
			{
				projectile.ai[1] += 1f;
				if (projectile.ai[1] > 40f)
				{
					projectile.Kill();
				}
			}
			base.AI();
		}
		public void MyMainGoal()
		{
			boom = true;
			Player player = Main.player[projectile.owner];
			int radius = (int)(100 * projectile.scale);
			Helper.CircleAOEDamage(projectile.Center, player, projectile.damage, projectile.knockBack, projectile.direction, Main.rand.Next(100) < player.rangedCrit, radius);
			Main.PlaySound(SoundID.Item14, projectile.Center);
			Main.PlaySound(SoundID.Item62, projectile.Center);
			Main.PlaySound(SoundID.Item91, projectile.Center);
			float particleCount = 160 * projectile.scale;
			int flameDust = SpellEffect.GetDust(0,6, Element);
			int sparkDust = SpellEffect.GetDust(1,133, Element);
			int smokeDust = 31;
			for (int i = 0; i < particleCount; i++) //fire circle
			{
				float angle = MathHelper.Lerp(0, 6.28f, i / particleCount);
				Vector2 position = projectile.Center + Vector2.One.RotatedBy(angle) * radius;
				Dust dust = Main.dust[Dust.NewDust(position, 0, 0, flameDust, 0, 0, 0, default, Main.rand.NextFloat(1.2f, 1.6f))];
				dust.noGravity = true;
				dust.noLight = false;
				dust.velocity = Vector2.Normalize(projectile.Center - position) * -0.5f;
			}
			for (int i = 0; i < particleCount; i++)
			{
				Vector2 position = new Vector2(projectile.position.X + projectile.width / 4, projectile.position.Y + projectile.height / 4);
				Dust dust = Main.dust[Dust.NewDust(position, projectile.width / 2, projectile.height / 2, flameDust, Main.rand.NextFloat(-3.5f, 3.5f), Main.rand.NextFloat(-3.5f, 3.5f), 100, default, Main.rand.NextFloat(1.2f, 2f))];
				dust.noGravity = true;
				dust.noLight = false;
				if (Main.rand.Next(2) == 0)
				{
					dust.scale *= 0.75f;
				}
			}
			particleCount = 60 * projectile.scale;
			for (int i = 0; i < particleCount; i++) //spark circle
			{
				float angle = MathHelper.Lerp(0, 6.28f, i / particleCount);
				Vector2 position = projectile.Center + Vector2.One.RotatedBy(angle * Main.rand.NextFloat(0.9f, 1.2f)) * radius;
				Dust dust = Main.dust[Dust.NewDust(position, 0, 0, sparkDust, 0, 0, 0, default, Main.rand.NextFloat(0.4f, 0.8f))];
				dust.noGravity = true;
				dust.noLight = false;
				dust.velocity = Vector2.Normalize(projectile.Center - position) * 0.25f;
			}
			particleCount = 30 * projectile.scale;
			for (int i = 0; i < particleCount; i++)
			{
				Vector2 position = new Vector2(projectile.position.X + projectile.width / 4, projectile.position.Y + projectile.height / 4);
				Dust dust = Main.dust[Dust.NewDust(position, projectile.width / 2, projectile.height / 2, sparkDust, Main.rand.NextFloat(-4.5f, 4.5f), Main.rand.NextFloat(-4.5f, 4.5f), 100, default, Main.rand.NextFloat(0.4f, 0.8f))];
				dust.noGravity = true;
				dust.noLight = false;
				if (Main.rand.Next(2) == 0)
				{
					dust.scale *= 0.5f;
				}
			}
			for (int i = 0; i < particleCount; i++)
			{
				Vector2 position = new Vector2(projectile.position.X + projectile.width / 4, projectile.position.Y + projectile.height / 4);

				Dust dust = Main.dust[Dust.NewDust(position, projectile.width / 2, projectile.height / 2, smokeDust, Main.rand.NextFloat(-2.5f, 2.5f), Main.rand.NextFloat(-2.5f, 2.5f), 100, new Color(), Main.rand.NextFloat(1.2f,3f))];
				dust.noGravity = true;
				dust.noLight = false;
			}
			for (int i = 0; i < particleCount; i++)
			{
				Gore gore = Main.gore[Gore.NewGore(projectile.Center, Vector2.Zero, Main.rand.Next(61, 64), 1f)];
				gore.velocity *= Main.rand.NextFloat(0.4f, 1.2f);
			}
		}
	}
}