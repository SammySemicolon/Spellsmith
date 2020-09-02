using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace Spellsmith
{
	public class Spellsmith : Mod
	{
		public static Spellsmith instance;
		//hotkeys
		public static ModHotKey Spell1;
		public static ModHotKey Spell2;
		public static ModHotKey Spell3;
		public static ModHotKey Spell4;
		public static ModHotKey NextSpell;
		public static ModHotKey PreviousSpell;

		//ui
		internal UserInterface abilityInterface;
		internal AbilityUI abilityUI;
		private GameTime lastGameTime;

		public override void Load()
		{
			instance = this;
			Spell1 = RegisterHotKey("Select Spell 1", "Z");
			Spell2 = RegisterHotKey("Select Spell 2", "X");
			Spell3 = RegisterHotKey("Select Spell 3", "C");
			Spell4 = RegisterHotKey("Select Spell 4", "V");
			NextSpell = RegisterHotKey("Select Next Spell", "");
			PreviousSpell = RegisterHotKey("Select Previous Spell", "");

			if (!Main.dedServ)
			{
				abilityInterface = new UserInterface();

				abilityUI = new AbilityUI();
				abilityUI.Activate(); // Activate calls Initialize() on the UIState if not initialized, then calls OnActivate and then calls Activate on every child element
			}
		}
		internal void ShowAbilityUI()
		{
			abilityInterface?.SetState(abilityUI);
		}

		internal void HideAbilityUI()
		{
			abilityInterface?.SetState(null);
		}
		public override void UpdateUI(GameTime gameTime)
		{
			lastGameTime = gameTime;
			if (abilityInterface?.CurrentState != null)
			{
				abilityInterface.Update(gameTime);
			}
		}
		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
			if (mouseTextIndex != -1)
			{
				layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
					"Spellsmith: Ability UI",
					delegate
					{
						if (lastGameTime != null && abilityInterface?.CurrentState != null)
						{
							abilityInterface.Draw(Main.spriteBatch, lastGameTime);
						}
						return true;
					},
					   InterfaceScaleType.UI));
			}
		}
		public override void Unload()
		{
			abilityUI.Deactivate();
			abilityUI = null;
		}
	}
	class AbilityUI : UIState
	{
		public override void OnInitialize()
		{
			UIPanel panel = new UIPanel();
			panel.Width.Set(300, 0);
			panel.Height.Set(300, 0);
			Append(panel);

			UIText header = new UIText("My UI Header");
			header.HAlign = 0.5f;
			header.Top.Set(15, 0);
			panel.Append(header);

			UIPanel button = new UIPanel(); // 1
			button.Width.Set(100, 0);
			button.Height.Set(50, 0);
			button.HAlign = 0.5f;
			button.Top.Set(25, 0); // 2
			button.OnClick += OnButtonClick; // 3
			panel.Append(button);

			UIText text = new UIText("Click me!");
			text.HAlign = text.VAlign = 0.5f; // 4
			button.Append(text); // 5
		}
		private void OnButtonClick(UIMouseEvent evt, UIElement listeningElement)
		{
			// We can do stuff in here!
		}
	}
}