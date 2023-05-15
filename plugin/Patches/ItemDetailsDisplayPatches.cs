using BetterInventory.Extensions;
using HarmonyLib;

namespace BetterInventory.Patches {
	[HarmonyPatch(typeof(ItemDetailsDisplay))]
	public static class ItemDetailsDisplayPatches {

		[HarmonyPatch(nameof(ItemDetailsDisplay.RefreshDetails)), HarmonyPostfix]
		private static void ItemDetailsDisplay_RefreshDetails_Postfix(ItemDetailsDisplay __instance) {
			Item item = __instance.m_lastItem;
			if (item == null) {
				return;
			}

			Character character = __instance.CharacterUI.TargetCharacter;
			int row = __instance.m_detailRows.Count;
			__instance.GetRow(row).SetInfo(LocalizationManager.Instance.GetLoc($"{BetterInventory.GUID}.item_detail.value"), item.RawBaseValue.ToString());
			row++;
			__instance.GetRow(row).SetInfo(LocalizationManager.Instance.GetLoc($"{BetterInventory.GUID}.item_detail.sell_price"), item.GetSellValue(character));
		}

	}
}