using Microsoft.Xna.Framework;
using Spellsmith.Items.EnchantedRunes;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellsmith.Projectiles.RuneProjectiles
{
    public class SpellProjectile : ModProjectile
    {
        public Color color;
        public SpellEffect casterEffect;
		public SpellEffect.SpellElement Element = SpellEffect.SpellElement.None;
		public override string Texture => "Spellsmith/Invisible";
    }
}