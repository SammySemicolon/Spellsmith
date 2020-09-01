using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ID;
using Terraria;
using System.Collections.Generic;
using Spellsmith.Items.EnchantedRunes;
using System.Linq;
using System;
using Microsoft.Xna.Framework;

namespace Spellsmith.Items
{
    public class SpellBlaster : ModItem
    {
        SpellRune spellRune;
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Basic Shooty Shoot");
        }
        public override void SetDefaults()
        {
            item.damage = 8;
            item.width = 36;
            item.height = 22;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 1;
            item.value = 10000;
            item.rare = ItemRarityID.Green;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.shoot = 10;
            item.shootSpeed = 16f;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Item ammo = (Item)player.inventory.GetValue(54);
            spellRune = (SpellRune)ammo.modItem;

            if (spellRune != null)
            {
                foreach (Effect effect in spellRune.effects)
                {
                    effect.RunSpell(player, this.item, new Vector2(speedX, speedY), item.shootSpeed, damage, knockBack);
                }
            }
            return false;
        }
    }
}