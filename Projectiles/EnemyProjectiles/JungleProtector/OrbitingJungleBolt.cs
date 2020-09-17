using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellsmith.Projectiles.EnemyProjectiles.JungleProtector
{
	public class OrbitingGreenBolt : ModProjectile
	{
		public Vector2 targetPosition;
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
		public float dist = 250;
		public float distChange = 0;
		public bool visual = false;
        public override bool CanHitPlayer(Player target)
        {
            return visual ? false : base.CanHitPlayer(target);
        }
        public override void AI()
		{
			projectile.ai[0] += 0.0085f * projectile.ai[1];
			dist -= distChange;
			double deg = projectile.ai[0];
			Vector2 cachedPosition = projectile.Center;
			projectile.position.X = targetPosition.X - (int)(Math.Cos(deg) * dist) - projectile.width / 2;
			projectile.position.Y = targetPosition.Y - (int)(Math.Sin(deg) * dist) - projectile.height / 2;
			projectile.rotation = Vector2.Normalize(projectile.Center - cachedPosition).ToRotation() + 1.57f;
		}
	}
	public class OrbitingPinkBolt : OrbitingGreenBolt
	{
		public override string Texture => "Spellsmith/Projectiles/EnemyProjectiles/JungleProtector/PinkBolt";
	}
}