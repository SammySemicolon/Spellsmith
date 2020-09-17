using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellsmith.Projectiles.EnemyProjectiles.JungleProtector
{
	public class DelayedGreenBolt : ModProjectile
	{
		public Vector2 targetPosition;
		bool touched = false;
		bool sent = false;
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}
		public override string Texture => "Spellsmith/Projectiles/EnemyProjectiles/JungleProtector/GreenBolt";
		public override void SetDefaults()
		{
			projectile.width = 22;
			projectile.height = 22;
			projectile.timeLeft = 1000;
			projectile.hostile = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.scale = 1f;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			spriteBatch.End();
			Main.spriteBatch.Begin(default, BlendState.Additive, default, default, default, default, Main.GameViewMatrix.EffectMatrix);
			Vector2 drawOrigin = new Vector2(projectile.width / 2, projectile.height / 2);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Vector2 glowPos = projectile.oldPos[k] - Main.screenPosition + new Vector2(0f, projectile.gfxOffY) + drawOrigin;
				float glowScale = 1 - ((float)k / projectile.oldPos.Length);
				Color finalColor = projectile.GetAlpha(Color.White) * ((float)(projectile.oldPos.Length - k) / projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], glowPos, null, finalColor, projectile.oldRot[k], drawOrigin, glowScale, SpriteEffects.None, 0f);
			}
			Main.spriteBatch.End();
			Main.spriteBatch.Begin();
			return false;
		}
		
		public override void AI()
		{
			Player target = Main.player[(int)projectile.ai[0]];
			if (!touched)
			{
				projectile.velocity = Vector2.Normalize(targetPosition - projectile.Center) * projectile.ai[1];
				if (Helper.CheckCircularCollision(targetPosition, projectile.Hitbox, 20))
				{
					touched = true;
				}
				projectile.rotation += 0.05f;
				projectile.timeLeft++;
			}
			else if (!sent)
            {
				projectile.velocity *= 0.9f;
				float desiredRotation = Vector2.Normalize(target.Center - projectile.Center).ToRotation() + 6.28f + 1.57f;
				projectile.rotation = MathHelper.Lerp(projectile.rotation, desiredRotation, 0.05f);
				if (projectile.timeLeft <= 900)
                {
					projectile.velocity = new Vector2(0, -1).RotatedBy(projectile.rotation) * projectile.ai[1];
					sent = true;
                }
            }
			else
            {
				projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
				projectile.velocity *= 1.01f;
            }
		}
	}
	public class DelayedPinkBolt : DelayedGreenBolt
	{
		public override string Texture => "Spellsmith/Projectiles/EnemyProjectiles/JungleProtector/PinkBolt";
	}
}