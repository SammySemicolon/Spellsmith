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
	public class FadingGreenBolt : ModProjectile
	{
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
			projectile.timeLeft = 100;
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
			if (projectile.timeLeft <= 20)
            {
				projectile.velocity *= 0.8f;
				projectile.scale -= 0.04f;
				if (projectile.scale <= 0)
                {
					projectile.Kill();
                }
            }
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
		}
	}
	public class FadingPinkBolt : FadingGreenBolt
	{
		public override string Texture => "Spellsmith/Projectiles/EnemyProjectiles/JungleProtector/PinkBolt";

	}
}