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
			DisplayName.SetDefault("Enchanted Rune: Arrow");
		}
		public override Effect effect => new ShootArrowEffect();
	}
	public class ShootArrowEffect : Effect
	{
		public ShootArrowEffect()
		{
			setup();
			manaCost = 4;
			tooltip = "Shoots an arrow";
		}
		public override SpellType Type => SpellType.Ranged;
		public override void DoEffect(Player player, Item item, Vector2 originalVelocity, float shootSpeed, int damage, float knockBack, int importantRun = 0, int totalImportantRuns = 0)
		{
			ShootProjectile(player, originalVelocity, shootSpeed, damage, knockBack, GetArrow(), importantRun, totalImportantRuns);
		}
		public int GetArrow()
		{
			List<int> arrows = new List<int>();
			if (modifiers.fieryModifier)
			{
				arrows.Add(ProjectileID.FireArrow);
			}
			if (modifiers.frostModifier)
			{
				arrows.Add(ProjectileID.FrostburnArrow);
			}
			if (modifiers.holyModifier)
			{
				arrows.Add(ProjectileID.HolyArrow);
			}
			if (modifiers.crimsonModifier)
			{
				arrows.Add(ProjectileID.IchorArrow);
			}
			if (modifiers.corruptModifier)
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