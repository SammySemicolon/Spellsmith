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
            item.channel = true;
            item.shoot = 10;
            item.shootSpeed = 16f;
        }
        public override bool CanUseItem(Player player)
        {
            setupBlaster(player);

            if (spellRune != null)
            {
                foreach (SpellEffect effect in spellRune.effects)
                {
                    if (!effect.CanRunSpell(player,item))
                    {
                        return false;
                    }
                }
            }
            return base.CanUseItem(player);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            SpellsmithPlayer modPlayer = player.GetModPlayer<SpellsmithPlayer>();
            setupBlaster(player);

            if (spellRune != null)
            {
                foreach (SpellEffect effect in spellRune.effects)
                {
                    if (!modPlayer.activeEffects.Contains(effect))
                    {
                        if (effect.CanRunSpell(player, item))
                        {
                            modPlayer.activeEffects.Add(effect);
                        }
                    }
                }
            }
            return false;
        }
        public void setupBlaster(Player player)
        {
            SpellsmithPlayer modPlayer = player.GetModPlayer<SpellsmithPlayer>();
            Item ammo = (Item)player.inventory.GetValue(54 + modPlayer.selectedSpell);
            spellRune = (SpellRune)ammo.modItem;
            modPlayer.activeBlaster = item;
        }
    }
}