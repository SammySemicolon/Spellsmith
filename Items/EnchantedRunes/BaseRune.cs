

using System.Collections.Generic;

namespace Spellsmith.Items.EnchantedRunes
{
	public class BaseRune : EnchantedRune
	{
		public override void SetStaticDefaults() 
		{
			Tooltip.SetDefault("Based");
		}
		public virtual void setup(Effect effect)
		{

		}
	}
}