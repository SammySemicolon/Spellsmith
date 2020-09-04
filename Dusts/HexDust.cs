using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace Spellsmith.Dusts
{
	public class HexDust : RGBDust
	{
		public override void OnSpawn(Dust dust)
		{
			startingColor = new Color(233, 145, 255);
			fadeOut = true;
			scaleChange = 0.05f;
			startingAlpha = 150;
            dust.frame = new Rectangle(0, 0, 11, 11);			
			base.OnSpawn(dust);
		}
        public override bool Update(Dust dust)
        {
			dust.color.R -= 3;
			dust.color.B -= 2;
            return base.Update(dust);
        }
    }
}