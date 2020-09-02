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
        public Effect casterEffect;
		public Effect.SpellElement Element = Effect.SpellElement.None;
		public void SetupColor(Color defaultColor)
		{
			switch (Element)
			{
				case (Effect.SpellElement.None):
					{
						color = defaultColor;
						break;
					}
				case (Effect.SpellElement.Fiery):
					{
						color = new Color(255, 103, 33);
						break;
					}
				case (Effect.SpellElement.Frost):
					{
						color = new Color(70, 206, 255);
						break;
					}
				case (Effect.SpellElement.Holy):
					{
						color = new Color(243, 145, 207);
						break;
					}
				case (Effect.SpellElement.Corrupt):
					{
						color = new Color(194, 236, 61);
						break;
					}

				case (Effect.SpellElement.Crimson):
					{
						color = new Color(199, 17, 17);
						break;
					}
			}
		}
		public int GetPrimaryDust(int defaultDust)
		{
			switch (Element)
			{
				case (Effect.SpellElement.None):
					{
						return defaultDust;
					}
				case (Effect.SpellElement.Fiery):
					{
						return 6;
					}
				case (Effect.SpellElement.Frost):
					{
						return 135;
					}
				case (Effect.SpellElement.Holy):
					{
						return 58;
					}
				case (Effect.SpellElement.Corrupt):
					{
						return 75;
					}
				case (Effect.SpellElement.Crimson):
					{
						return 170;
					}
			}
			return 0;
		}
		public int GetSecondaryDust(int defaultDust)
		{
			switch (Element)
			{
				case (Effect.SpellElement.None):
					{
						return defaultDust;
					}
				case (Effect.SpellElement.Fiery):
					{
						return 127;
					}
				case (Effect.SpellElement.Frost):
					{
						return 176;
					}
				case (Effect.SpellElement.Holy):
					{
						return 57;
					}
				case (Effect.SpellElement.Corrupt):
					{
						return 14;
					}
				case (Effect.SpellElement.Crimson):
					{
						return 125;
					}
			}
			return 0;
		}
		public override string Texture => "Spellsmith/Invisible";
    }
}