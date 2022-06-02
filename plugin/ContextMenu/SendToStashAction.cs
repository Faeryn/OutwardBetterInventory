using UnityEngine;

namespace BetterInventory.ContextMenu {
	public class SendToStashAction : ContextMenuAction {
		public SendToStashAction(int id) : base(id, "Send To Stash") {
		}

		public override bool IsActive(GameObject pointerPress) {
			return true;
		}

		public override void ExecuteAction(ContextMenuOptions contextMenu) {
			ItemDisplayOptionPanel itemDisplayOptionPanel = contextMenu as ItemDisplayOptionPanel;
			if (itemDisplayOptionPanel != null) {
				ItemDisplay activatedItemDisplay = itemDisplayOptionPanel.m_activatedItemDisplay;
				if (activatedItemDisplay!= null && !activatedItemDisplay.IsEmpty) {
					TrySendToStash(activatedItemDisplay);
				}
			}
		}

		private ItemContainer FindStash() {
			return null;
		}
		
		private void TrySendToStash(ItemDisplay itemDisplay) {
			itemDisplay.TryMoveTo(FindStash());
		}
	}
}