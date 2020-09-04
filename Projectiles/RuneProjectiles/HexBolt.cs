using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spellsmith.Dusts;
using Spellsmith.Items.EnchantedRunes;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Spellsmith.Projectiles.RuneProjectiles
{
	public class HexBolt : SpellProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 20;
			projectile.height = 20;
			projectile.timeLeft = 100;
			projectile.penetrate = -1;
			projectile.extraUpdates = 3;
			projectile.friendly = true;
			projectile.magic = true;
			projectile.tileCollide = true;
			projectile.ignoreWater = true;
			projectile.scale = 1f;
		}
		public override string Texture => "Spellsmith/Invisible";

		public override void AI()
		{
			base.AI();

			int hexDust = SpellEffect.GetDust(0, 27, Element);
			int orbitDust = SpellEffect.GetDust(1, 159, Element);
			Dust hex = Dust.NewDustPerfect(projectile.Center, hexDust, -projectile.velocity * 0.2f, Main.rand.Next(100, 175), default, Main.rand.NextFloat(0.8f, 1.2f));
			hex.noGravity = true;
			hex.scale += projectile.scale;
			float rot = Vector2.Normalize(projectile.velocity).ToRotation();
			float radius = 10 * projectile.scale;
			float x = projectile.Center.X + (float)Math.Sin(rot) * ((float)Math.Sin((float)projectile.timeLeft / 5) * radius);
			float y = projectile.Center.Y + (float)Math.Cos(rot) * ((float)Math.Sin((float)projectile.timeLeft / 5) * -radius);
			Vector2 orbitPos = new Vector2(x, y);
			Dust orbit = Dust.NewDustPerfect(orbitPos, orbitDust, -projectile.velocity * 0.1f, Main.rand.Next(100, 175), default, Main.rand.NextFloat(0.5f, 0.8f));
			orbit.noGravity = true;
			orbit.scale += projectile.scale;
		}
	}
}