using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace Spellsmith.Dusts
{
	public abstract class RGBDust : ModDust
	{
		public Color startingColor;
		public bool fadeOut = false; 
		public float scaleChange;
		public float startingScale;
		public int startingAlpha;
		public override void OnSpawn(Dust dust)
		{
			dust.noGravity = true;
			dust.noLight = false;
			startingScale = dust.scale;
			dust.color = startingColor;
			dust.alpha = startingAlpha;
		}
		public override Color? GetAlpha(Dust dust, Color lightColor)
		{
			return dust.color * ((255 - dust.alpha) / 255f);
		}
		public override bool Update(Dust dust)
		{
			dust.position += dust.velocity;
			dust.rotation = dust.velocity.ToRotation();
			dust.scale -= scaleChange;
			if (fadeOut)
			{
				dust.alpha = (int)MathHelper.Lerp(255, startingAlpha, dust.scale / startingScale);
			}
			if (dust.scale < scaleChange)
			{
				dust.active = false;
			}
			else
			{
				float strength = dust.scale / 2f;
				Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), dust.color.R / 255f * 0.5f * strength, dust.color.G / 255f * 0.5f * strength, dust.color.B / 255f * 0.5f * strength);
			}
			return false;
		}
    }
}