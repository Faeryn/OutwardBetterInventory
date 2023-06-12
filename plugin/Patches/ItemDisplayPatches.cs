using System;
using System.Runtime.CompilerServices;
using BetterInventory.Extensions;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace BetterInventory.Patches {
	
	[HarmonyPatch(typeof(ItemDisplay))]
	public static class ItemDisplayPatches {
		
		private static ConditionalWeakTable<ItemDisplay, GameObject> coinIconRefs = new ConditionalWeakTable<ItemDisplay, GameObject>();

		[HarmonyPatch(nameof(ItemDisplay.UpdateValueDisplay)), HarmonyPostfix]
		private static void ItemDisplay_UpdateValueDisplay_Postfix(ItemDisplay __instance) {
			ItemDisplayInfo itemDisplayInfo = BetterInventory.ItemDisplayValue.Value;
			if (itemDisplayInfo == ItemDisplayInfo.Off) {
				return;
			}
			
			if (!__instance.RefItem || __instance.RefItem is Skill) {
				return;
			}

			Text valueDisplay = __instance.m_lblValue;
			
			if (!__instance.m_valueHolder || !valueDisplay || !__instance.CharacterUI) {
				return;
			}
			
			Character character = __instance.CharacterUI.TargetCharacter;
			
			if (!character) {
				return;
			}

			GameObject coinIcon = coinIconRefs.GetValue(__instance, key => key.m_valueHolder.transform.Find("CoinIcon")?.gameObject);

			Func<CharacterUI.MenuScreens, bool> isMenuDisplayed = __instance.CharacterUI.GetIsMenuDisplayed;

			if (!isMenuDisplayed(CharacterUI.MenuScreens.Inventory) 
				&& !isMenuDisplayed(CharacterUI.MenuScreens.Stash) ) {
				// Enable only in inventory and stash
				valueDisplay.color = Color.white;
				if (coinIcon && !coinIcon.activeSelf) {
					coinIcon.SetActive(true);
				}
				return;
			}

			if (!__instance.m_valueHolder.activeSelf) {
				__instance.m_valueHolder.SetActive(true);
			}

			Item item = __instance.RefItem;
			bool highlight = false;
			bool icon = true;
			string text = "";

			switch (itemDisplayInfo) {
				case ItemDisplayInfo.Value:
					text = item.RawBaseValue.ToString();
					highlight = item.IsSellModifierOverridden();
					break;
				case ItemDisplayInfo.SellPrice:
					text = item.GetSellValue(character).ToString();
					highlight = item.IsSellModifierOverridden();
					break;
				case ItemDisplayInfo.SellPricePerLb:
					int sellPrice = item.GetSellValue(character);
					float weight = item.RawWeight;
					text = weight > 0 && sellPrice > 0 ? (sellPrice / weight).ToString("0.#") : "0";
					highlight = item.IsSellModifierOverridden();
					break;
				case ItemDisplayInfo.Weight:
					text = item.RawWeight.ToString("0.0");
					icon = false;
					break;
			}
			
			valueDisplay.text = text;
			valueDisplay.color = highlight ? Color.yellow : Color.white;
			if (coinIcon && coinIcon.activeSelf != icon) {
				coinIcon.SetActive(icon);
			}
		}
		
	}
}