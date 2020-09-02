using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace Spellsmith.Dusts
{
	public class AirstrikeDust : RGBDust
	{
        public override void OnSpawn(Dust dust)
        {
			startingColor = new Color(255, 225, 163);
			fadeOutColor = new Color(255, 142, 100);
			fadeOut = true;
			scaleChange = 0.05f;
			startingScale = 1.5f;
            dust.alpha = 150;
            base.OnSpawn(dust);
        }
        public override bool Update(Dust dust)
        {
            dust.velocity *= 0.9f;
            return base.Update(dust);
        }
    }
}