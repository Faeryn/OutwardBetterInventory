using BetterInventory.Extensions;
using HarmonyLib;
using UnityEngine;

namespace BetterInventory.Patches {
	
	[HarmonyPatch(typeof(ItemDisplay))]
	public static class ItemDisplayPatches {
		
		[HarmonyPatch(nameof(ItemDisplay.UpdateValueDisplay)), HarmonyPostfix]
		private static void ItemDisplay_UpdateValueDisplay_Postfix(ItemDisplay __instance) {
			if (!BetterInventory.ShowItemValueEnabled.Value) {
				return;
			}
			
			if (!__instance.RefItem || __instance.RefItem is Skill) {
				return;
			}

			if (__instance.CharacterUI && __instance.CharacterUI.GetIsMenuDisplayed(CharacterUI.MenuScreens.Shop)) {
				// Disable in shops
				__instance.m_lblValue.color = Color.white;
				return;
			}

			if (!__instance.m_valueHolder.activeSelf) {
				__instance.m_valueHolder.SetActive(true);
			}

			Item item = __instance.RefItem;
			__instance.m_lblValue.text = item.RawBaseValue.ToString();
			__instance.m_lblValue.color = item.IsSellModifierOverridden() ? Color.yellow : Color.white;
		}
		
	}
}