using Microsoft.Xna.Framework;
using Spellsmith.Projectiles.RuneProjectiles;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellsmith.Items.EnchantedRunes.Effects
{
	public class AirstrikeRune : EffectRune
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Nuclear Bombardment lol");
		}
		public override Effect effect => new AirstrikeEffect();
	}
	public class AirstrikeEffect : Effect
	{
		public override SpellType Type => SpellType.Ranged;
        public override SpellStyle Style => SpellStyle.HoldRelease;
		public override void DoEffect(Entity entity, Item item, Vector2 originalVelocity, float shootSpeed, int damage, float knockBack, int importantRun = 0, int totalImportantRuns = 0)
		{
			ShootSkyProjectile(entity, item, 1200 * (importantRun + 1), 6, damage, knockBack, ModContent.ProjectileType<Airstrike>(), importantRun, totalImportantRuns);
		}
		public override void ModifyProjectile(Entity entity, Projectile projectile, Vector2 executePosition = default)
        {
            base.ModifyProjectile(entity, projectile, executePosition);
			Airstrike airstrike = projectile.modProjectile as Airstrike;
			airstrike.Y = executePosition.Y;
			projectile.netUpdate = true;
			projectile.ai[0] += Main.rand.NextFloat(0, 621f);
			projectile.rotation += Main.rand.NextFloat(0, 621f);
		}
    }
}