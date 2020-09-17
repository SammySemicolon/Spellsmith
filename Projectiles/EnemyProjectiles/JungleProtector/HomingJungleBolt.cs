using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spellsmith.Items.EnchantedRunes;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellsmith.Projectiles.EnemyProjectiles.JungleProtector
{
	public class HomingGreenBolt : ModProjectile
	{
		public bool touched = false;
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
			projectile.extraUpdates = 1;
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
				float glowScale = (1 - ((float)k / projectile.oldPos.Length)) * projectile.scale;
				Color finalColor = projectile.GetAlpha(Color.White) * ((float)(projectile.oldPos.Length - k) / projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], glowPos, null, finalColor, projectile.oldRot[k], drawOrigin, glowScale, SpriteEffects.None, 0f);
			}
			Main.spriteBatch.End();
			Main.spriteBatch.Begin();
			return false;
		}
        public override void AI()
		{
			projectile.ai[0] += 0.02f;
			if (!touched)
			{
				Vector2 targetPosition = Main.player[(int)projectile.ai[1]].Center;
				projectile.timeLeft++;
				float sin = Math.Abs((float)Math.Sin(projectile.ai[0]) * 0.5f);
				projectile.velocity += Vector2.Normalize(targetPosition - projectile.Center) * sin;
				projectile.velocity = Vector2.Normalize(projectile.velocity) * projectile.ai[0];
				if (Helper.CheckCircularCollision(targetPosition, projectile.Hitbox, 80))
                {
					touched = true;
                }
			}
			else
            {
				projectile.velocity *= 0.98f;
				projectile.scale -= 0.02f;
				if (projectile.scale <= 0)
				{
					projectile.Kill();
				}
			}
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
		}
	}
	public class HomingPinkBolt : HomingGreenBolt
	{
		public override string Texture => "Spellsmith/Projectiles/EnemyProjectiles/JungleProtector/PinkBolt";
	}
}