using Terraria.ModLoader;
using Terraria.ID;

namespace Spellsmith.Items.EnchantedRunes
{
	public class EnchantedRune : ModItem
	{
		public override void SetStaticDefaults() 
		{
			Tooltip.SetDefault("Idk");
		}

		public override void SetDefaults() 
		{
			item.width = 40;
			item.height = 40;
			item.value = 1000;
			item.rare = ItemRarityID.Green;
		}
    }
}