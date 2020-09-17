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
	public class BlastGreenBolt : ModProjectile
	{
		public override string Texture => "Spellsmith/Invisible";
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
		public override bool CanHitPlayer(Player target)
		{
			return false;
		}
		public override void AI()
		{
			if (projectile.ai[0] == 0)
			{
				bool direction = Main.rand.NextBool();
				for (int i = 0; i < 4; i++)
				{
					float rot = MathHelper.Lerp(0, 6.28f, ((float)i) / 4);
					Projectile bolt = Projectile.NewProjectileDirect(projectile.Center, Vector2.Zero, ModContent.ProjectileType<OrbitingGreenBolt>(), 50, 2f);
					bolt.ai[0] = rot + 1.57f;
					bolt.ai[1] = direction ? -6 : 6;
					bolt.timeLeft = 80;
					OrbitingGreenBolt orbitingGreenBolt = bolt.modProjectile as OrbitingGreenBolt;
					orbitingGreenBolt.targetPosition = projectile.Center;
					orbitingGreenBolt.distChange = 1;
					orbitingGreenBolt.dist = 80;
					orbitingGreenBolt.visual = true;
				}
			}
			if (projectile.ai[0] == 80)
			{
				int boltCount = 20;
				for (int i = 0; i < boltCount; i++)
				{
					float rot = MathHelper.Lerp(0, 6.28f, ((float)i) / boltCount);
					Vector2 velocity = new Vector2(0, 8).RotatedBy(rot);
					Projectile.NewProjectileDirect(projectile.Center, velocity, ModContent.ProjectileType<FadingGreenBolt>(), 50, 2f);
				}
				projectile.Kill();
			}
			projectile.ai[0]++;
		}
	}
	public class BlastPinkBolt : BlastGreenBolt
	{
		public override string Texture => "Spellsmith/Invisible";
	}
}