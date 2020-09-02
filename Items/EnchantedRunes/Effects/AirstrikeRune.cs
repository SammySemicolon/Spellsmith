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
			DisplayName.SetDefault("Enchanted Rune: 'Airstrike'");
		}
		public override Effect effect => new AirstrikeEffect();
	}
	public class AirstrikeEffect : Effect
	{
		public AirstrikeEffect()
        {
			maxCooldown = 600;
			manaCost = 40;
			damageMultiplier = 2f;
			tooltip = "Calls down an orbital airstrike at your mouse position";
		}
		public override SpellType Type => SpellType.Ranged;
        public override SpellStyle Style => SpellStyle.HoldRelease;
		public override void DoEffect(Player player, Item item, Vector2 originalVelocity, float shootSpeed, int damage, float knockBack, int importantRun = 0, int totalImportantRuns = 0)
		{
			ShootSkyProjectile(player, item, 1200 * (importantRun + 1), 12, damage, knockBack, ModContent.ProjectileType<Airstrike>(), importantRun, totalImportantRuns);
		}
		public override void ModifyProjectile(Player player, Projectile projectile, Vector2 executePosition = default)
        {
            base.ModifyProjectile(player, projectile, executePosition);
			Airstrike airstrike = projectile.modProjectile as Airstrike;
			airstrike.Y = executePosition.Y;
			projectile.netUpdate = true;
			projectile.ai[0] += Main.rand.NextFloat(0, 621f);
			projectile.rotation += Main.rand.NextFloat(0, 621f);
		}
    }
}