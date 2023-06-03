using BetterInventory.Extensions;
using HarmonyLib;
using UnityEngine;

namespace BetterInventory.Patches {
	[HarmonyPatch(typeof(ItemDetailsDisplay))]
	public static class ItemDetailsDisplayPatches {

		[HarmonyPatch(nameof(ItemDetailsDisplay.ShowDetails)), HarmonyPostfix]
		private static void ItemDetailsDisplay_ShowDetails_Postfix(ItemDetailsDisplay __instance, int detailCount, ItemDetailsDisplay.DisplayedInfos[] infos) {
			Item item = __instance.m_lastItem;
			if (item == null || item is Skill) {
				return;
			}

			Character character = __instance.CharacterUI.TargetCharacter;
			if (character == null) {
				return;
			}
			int row = Mathf.Min(detailCount, __instance.m_detailRows.Count);
			__instance.GetRow(row).SetInfo(LocalizationManager.Instance.GetLoc($"{BetterInventory.GUID}.item_detail.value"), item.RawBaseValue.ToString());
			row++;
			int sellPrice = item.GetSellValue(character);
			float weight = item.RawWeight;
			string silverPerLb = (sellPrice / weight).ToString("0.#");
			__instance.GetRow(row).SetInfo(LocalizationManager.Instance.GetLoc($"{BetterInventory.GUID}.item_detail.sell_price"), $"{sellPrice}" + (weight > 0 && sellPrice > 0 ? $" ({silverPerLb}/lb)" : ""));
		}

	}
}