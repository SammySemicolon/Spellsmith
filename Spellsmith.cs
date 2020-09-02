using Terraria.ModLoader;

namespace Spellsmith
{
	public class Spellsmith : Mod
	{

		public static Spellsmith instance;
		public static ModHotKey Spell1;
		public static ModHotKey Spell2;
		public static ModHotKey Spell3;
		public static ModHotKey Spell4;
		public static ModHotKey NextSpell;
		public static ModHotKey PreviousSpell;
		public override void Load()
		{
			instance = this;
			Spell1 = RegisterHotKey("Select Spell 1", "Z");
			Spell2 = RegisterHotKey("Select Spell 2", "X");
			Spell3 = RegisterHotKey("Select Spell 3", "C");
			Spell4 = RegisterHotKey("Select Spell 4", "V");
			NextSpell = RegisterHotKey("Select Next Spell", "");
			PreviousSpell = RegisterHotKey("Select Previous Spell", "");
		}
	}
}