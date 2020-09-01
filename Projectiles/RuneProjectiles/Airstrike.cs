using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Spellsmith.Projectiles.RuneProjectiles
{
	public class Airstrike : SpellProjectile
	{
		public Color color = new Color(255, 86, 123);
		public float Y = 1000000;
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 80;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			projectile.width = 200;
			projectile.height = 200;
			projectile.timeLeft = 10;
			projectile.penetrate = -1;
			projectile.extraUpdates = 12;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.scale = 1f;
		}
		public override string Texture => "Spellsmith/Invisible";
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            spriteBatch.End();
            Main.spriteBatch.Begin(default, BlendState.Additive, default, default, default, default, Main.GameViewMatrix.ZoomMatrix);

            Vector2 drawOrigin = new Vector2(projectile.width / 2, projectile.height / 2);
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                Vector2 glowPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color finalColor = color * ((float)(projectile.oldPos.Length - k) / projectile.oldPos.Length);
                float glowScale = (projectile.scale - projectile.scale * k / projectile.oldPos.Length) * 0.1f;
                spriteBatch.Draw(GetTexture("Spellsmith/VFX/Glow"), glowPos, null, finalColor, 0, drawOrigin, glowScale, SpriteEffects.None, 0f);
            }
            Vector2 flashPos = projectile.position - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
			float sin = Math.Abs((float)Math.Sin(projectile.ai[0]));

			float flashScale = projectile.scale * 0.6f + sin * 0.1f;
            spriteBatch.Draw(GetTexture("Spellsmith/VFX/Flash"), flashPos, null, color, projectile.rotation, drawOrigin, flashScale, SpriteEffects.None, 0f);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin();
            return false;
        }
        public override bool? CanHitNPC(NPC target)
        {
			return false;
        }
        public override void AI()
		{
			projectile.timeLeft++;
			projectile.ai[0] += 0.02f;
			if (projectile.scale < 0.2f)
            {
				projectile.scale += 0.01f;
			}
			projectile.rotation += 0.01f;
			if (Y < projectile.position.Y)
            {
				MyMainGoal();
				projectile.Kill();
            }
			base.AI();
        }
		public void MyMainGoal()
        {
			Main.NewText("My main goal is to blow up");
        }
    }
}