using Microsoft.Xna.Framework;
using Spellsmith.Items.EnchantedRunes;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;

namespace Spellsmith.Projectiles.RuneProjectiles
{
    public class HexBolt : SpellProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 20;
			projectile.height = 20;
			projectile.timeLeft = 100;
			projectile.penetrate = 5;
			projectile.extraUpdates = 3;
			projectile.friendly = true;
			projectile.magic = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.scale = 1f;
		}
		public override string Texture => "Spellsmith/Invisible";

		bool hit = false;
		NPC target;
		List<NPC> hitTargets = new List<NPC>();
        public override bool? CanHitNPC(NPC target)
        {
			if (hitTargets.Contains(target))
            {
				return false;
            }
            return base.CanHitNPC(target);
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
			hit = true;
			hitTargets.Add(target);
			projectile.velocity = new Vector2(Main.rand.NextFloat(-projectile.velocity.X, projectile.velocity.X), Main.rand.NextFloat(-projectile.velocity.Y, projectile.velocity.Y));
			this.target = null;
        }
        public override void AI()
		{
			base.AI();
			int hexDust = SpellEffect.GetDust(0, 27, Element);
			int orbitDust = SpellEffect.GetDust(1, 159, Element);
			Dust hex = Dust.NewDustPerfect(projectile.Center, hexDust, -projectile.velocity * 0.2f, Main.rand.Next(100, 175), default, Main.rand.NextFloat(0.8f, 1.2f));
			hex.noGravity = true;
			hex.noLight = false;
			hex.scale += projectile.scale;
			float rot = Vector2.Normalize(projectile.velocity).ToRotation();
			float radius = 10 * projectile.scale;
			float angle = projectile.ai[0] += 0.2f;
			float x = projectile.Center.X + (float)Math.Sin(rot) * ((float)Math.Sin(angle) * radius);
			float y = projectile.Center.Y + (float)Math.Cos(rot) * ((float)Math.Sin(angle) * -radius);
			Vector2 orbitPos = new Vector2(x, y);
			Dust orbit = Dust.NewDustPerfect(orbitPos, orbitDust, -projectile.velocity * 0.1f, Main.rand.Next(100, 175), default, Main.rand.NextFloat(0.5f, 0.8f));
			orbit.noGravity = true;
			orbit.noLight = false;
			orbit.scale += projectile.scale / 2;
			if (hit)
			{
				if (target != null && Helper.CanHomeIn(target))
				{
					projectile.timeLeft++;
					float sin = (float)Math.Sin(projectile.ai[0] * 0.2f) * 0.4f;
					projectile.velocity += Vector2.Normalize(target.Center - projectile.Center) * ((projectile.ai[1] / 24) + sin);
					projectile.velocity = Vector2.Normalize(projectile.velocity) * (projectile.ai[1] + sin);
				}
				else
				{
					foreach (NPC npc in Main.npc.Where(n => Helper.CanHomeIn(n) && !hitTargets.Contains(n) && Helper.CheckCircularCollision(projectile.Center, n.Hitbox, 800)))
					{
						if (target == null)
						{
							target = npc;
						}
						else if (Vector2.Distance(npc.Center, projectile.Center) < Vector2.Distance(target.Center, projectile.Center))
						{
							target = npc;
						}
					}
				}
			}
		}
	}
}