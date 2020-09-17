using Microsoft.Xna.Framework;
using Spellsmith.Items.EnchantedRunes;
using Terraria;
using Terraria.ModLoader;

namespace Spellsmith.Projectiles.RuneProjectiles
{
    public class SpellProjectile : ModProjectile
    {
        public Color color;
        public SpellEffect casterEffect;
		public SpellEffect.SpellElement Element = SpellEffect.SpellElement.None;
		public override string Texture => "Spellsmith/Invisible";
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
            SpellEffect.SpellEffectHit(target, Main.player[projectile.owner], damage, 1f, Element);
        }
    }
}