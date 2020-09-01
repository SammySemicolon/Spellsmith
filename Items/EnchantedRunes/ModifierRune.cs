using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
namespace Spellsmith.Items.EnchantedRunes
{
    public class ModifierRune : BaseRune
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Modify");
        }
        public DefaultFuckUpEffect fuckUpEffect => new DefaultFuckUpEffect();
    }
    public class DefaultFuckUpEffect : Effect
    {
        public override void DoEffect(Entity entity, Item item, Vector2 originalVelocity, float shootSpeed, int damage, float knockBack, int importantRun = 0, int totalImportantRuns = -10)
        {
            if (entity is Player)
            {
                Player player = (Player)entity;
                string deathMessage = player.name + " fucked up their spell";
                if (fiery)
                {
                    deathMessage = player.name + " erupted into flames";
                }
                if (frost)
                {
                    deathMessage = player.name + " became a cube of ice";
                }
                if (holy)
                {
                    deathMessage = player.name + " turned into rainbow dust";
                }
                if (crimson)
                {
                    deathMessage = player.name + " became a clump of flesh";
                }
                if (corrupt)
                {
                    deathMessage = player.name + " sacrificed themselves to the dark gods";
                }
                player.KillMe(PlayerDeathReason.ByCustomReason(deathMessage), player.statLifeMax2, 0);
            }
        }
    }
}