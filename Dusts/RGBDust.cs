using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace Spellsmith.Dusts
{
	public abstract class RGBDust : ModDust
	{
		public static Color defaultFadeOutColor = new Color(0,0,0);
		public Color startingColor;
		public Color fadeOutColor;
		public bool fadeOut = false; 
		public float scaleChange;
		public float startingScale;
		public override void OnSpawn(Dust dust)
		{
			dust.noGravity = true;
			dust.noLight = true;
			dust.scale = startingScale;
			dust.color = startingColor;
		}
		public override Color? GetAlpha(Dust dust, Color lightColor)
		{
			return dust.color;
		}
		public override bool Update(Dust dust)
		{
			dust.position += dust.velocity;
			dust.rotation += dust.velocity.X;
			dust.scale -= scaleChange;
			if (dust.scale < scaleChange)
			{
				dust.active = false;
			}
			else
			{
				if (fadeOut)
				{
					if (fadeOutColor == defaultFadeOutColor)
					{
						dust.color = startingColor * (dust.scale / startingScale);
					}
					else
                    {
						byte R = (byte)MathHelper.Lerp(fadeOutColor.R, startingColor.R, dust.scale / startingScale);
						byte G = (byte)MathHelper.Lerp(fadeOutColor.G, startingColor.G, dust.scale / startingScale);
						byte B = (byte)MathHelper.Lerp(fadeOutColor.B, startingColor.B, dust.scale / startingScale);
						dust.color = new Color(R, G, B);
					}
				}
				float strength = dust.scale / 2f;
				Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), dust.color.R / 255f * 0.5f * strength, dust.color.G / 255f * 0.5f * strength, dust.color.B / 255f * 0.5f * strength);
			}
			return false;
		}
    }
}