using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace BetterInventory {
	[BepInPlugin(GUID, NAME, VERSION)]
	public class BetterInventory : BaseUnityPlugin {
		public const string GUID = "faeryn.betterinventory";
		public const string NAME = "BetterInventory";
		public const string VERSION = "1.2.5";
		private const string DISPLAY_NAME = "Better Inventory";
		internal static ManualLogSource Log;
		
		public static ConfigEntry<bool> SalvageEnabled;
		public static ConfigEntry<bool> SendToStashEnabled;
		public static ConfigEntry<bool> SendToHostStashEnabled;
		public static ConfigEntry<bool> SendToOtherEnabled;
		public static ConfigEntry<float> SendToOtherMaxDistance;
		public static ConfigEntry<ItemDisplayInfo> ItemDisplayValue;
		
		internal void Awake() {
			Log = this.Logger;
			Log.LogMessage($"Starting {NAME} {VERSION}");
			InitializeConfig();
			new Harmony(GUID).PatchAll();
		}
		
		private void InitializeConfig() {
			SalvageEnabled = Config.Bind(DISPLAY_NAME, "Salvage", true, "Enables the 'Salvage' action on items");
			SendToStashEnabled = Config.Bind(DISPLAY_NAME, "Send to Stash", true, "Enables the 'Send to Stash' action on items while in town where you own a stash");
			SendToHostStashEnabled = Config.Bind(DISPLAY_NAME, "Send to Host Stash", true, "Enables the 'Send to Host's Stash' action on items while in town where the host owns a stash");
			SendToOtherEnabled = Config.Bind(DISPLAY_NAME, "Send to Other Player", true, "Enables the 'Send to Other Player' action on items in multiplayer");
			SendToOtherMaxDistance = Config.Bind(DISPLAY_NAME, "Send to Other Player maximum distance", 10f, "Maximum distance between you and the recipient (in metres)");
			ItemDisplayValue = Config.Bind(DISPLAY_NAME, "Item Display Value", ItemDisplayInfo.SellPrice, "Shows this value on item display panels in the inventory");
		}
	}
}