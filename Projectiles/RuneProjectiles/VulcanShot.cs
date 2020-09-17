using Spellsmith.Items.EnchantedRunes;
using Terraria;

namespace Spellsmith.Projectiles.RuneProjectiles
{
    public class VulcanShot : SpellProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 40;
			projectile.height = 40;
			projectile.timeLeft = 400;
			projectile.penetrate = 1;
			projectile.extraUpdates = 8;
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
			int boltDust = SpellEffect.GetDust(0, 228, Element);
			if (Main.rand.NextBool())
            {
				boltDust = SpellEffect.GetDust(1, 269, Element);
            }
			Dust bolt = Dust.NewDustPerfect(projectile.Center, boltDust, projectile.velocity * 0.5f, Main.rand.Next(100, 175), default, Main.rand.NextFloat(0.6f, 1f));
			bolt.noGravity = true;
			bolt.noLight = false;
			bolt.scale += projectile.scale;
		}
	}
}