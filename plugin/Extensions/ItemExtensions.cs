using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace BetterInventory.Extensions {
	public static class ItemExtensions {
		private static ConditionalWeakTable<Item, ItemExt> Exts = new ConditionalWeakTable<Item, ItemExt>();

		private static ItemExt Ext(this Item item) {
			if (!Exts.TryGetValue(item, out ItemExt ext)) {
				ext = new ItemExt();
				Exts.Add(item, ext);
			}
			return ext;
		}
		
		private static ItemExt TryExt(this Item item) {
			if (!Exts.TryGetValue(item, out ItemExt ext)) {
				return null;
			}
			return ext;
		}
		
		public static bool HasTemporaryDisplayName(this Item item) {
			return item.TryExt()?.TemporaryDisplayName != null;
		}
		
		public static void SetTemporaryDisplayName(this Item item, string name) {
			item.Ext().TemporaryDisplayName = name;
		}
		
		public static void ClearTemporaryDisplayName(this Item item) {
			ItemExt ext = item.TryExt();
			if (ext != null) {
				ext.TemporaryDisplayName = null;
			}
		}

		public static string GetTemporaryDisplayName(this Item item) {
			return item.TryExt()?.TemporaryDisplayName;
		}
		
		public static void PatchTemporaryDisplayName(this Item item, ref string displayName) {
			ItemExt ext = item.Ext();
			if (ext == null) {
				return;
			}
			string tempDisplayName = ext.TemporaryDisplayName;
			if (tempDisplayName != null) {
				displayName = tempDisplayName;
			}
			ext.TemporaryDisplayName = null;
		}
		
		public static bool IsSellModifierOverridden(this Item item) {
			return Math.Abs(item.m_overrideSellModifier + 1.0) > 0.0001f;
		}
		
		public static int GetSellValue(this Item item, Character player) {
			float num2 = item.IsSellModifierOverridden() ? item.m_overrideSellModifier : (1f + player.GetItemSellPriceModifier(null, item)) * 0.3f;
			return Mathf.RoundToInt(item.RawCurrentValue * num2);
		}

	}
}