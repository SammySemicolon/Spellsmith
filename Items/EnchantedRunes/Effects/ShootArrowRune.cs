using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace Spellsmith.Items.EnchantedRunes.Effects
{
	public class ShootArrowRune : EffectRune
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Shoot da Arrow");
		}
		public override Effect effect => new ShootArrowEffect();
	}
	public class ShootArrowEffect : Effect
	{
        public override SpellType Type => SpellType.Ranged;
		public override void DoEffect(Entity entity, Item item, Vector2 originalVelocity, float shootSpeed, int damage, float knockBack, int importantRun = 0, int totalImportantRuns = 0)
		{
			ShootProjectile(entity, originalVelocity, shootSpeed, damage, knockBack, GetArrow(), importantRun, totalImportantRuns);
		}
		public int GetArrow()
		{
			List<int> arrows = new List<int>();
			if (fiery)
			{
				arrows.Add(ProjectileID.FireArrow);
			}
			if (frost)
			{
				arrows.Add(ProjectileID.FrostburnArrow);
			}
			if (holy)
			{
				arrows.Add(ProjectileID.HolyArrow);
			}
			if (crimson)
			{
				arrows.Add(ProjectileID.IchorArrow);
			}
			if (corrupt)
			{
				arrows.Add(ProjectileID.CursedArrow);
			}
			if (arrows.Count > 0)
			{
				return arrows[Main.rand.Next(arrows.Count)];
			}
			return ProjectileID.WoodenArrowFriendly;
		}
	}
}