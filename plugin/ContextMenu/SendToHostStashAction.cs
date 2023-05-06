using UnityEngine;

namespace BetterInventory.ContextMenu {
	public class SendToHostStashAction : SendToStashAction {

		public override string GetText(ContextMenuOptions contextMenu) {
			return LocalizationManager.Instance.GetLoc($"{BetterInventory.GUID}.action.send_to_host_stash.text");
		}
		
		protected override bool IsActive(GameObject pointerPress, ItemDisplay itemDisplay, Item item, bool isCurrency) {
			return !IsHost() && BetterInventory.SendToHostStashEnabled.Value && TryGetStash(out _) && (isCurrency || item.IsChildToPlayer);
		}

		protected override void ExecuteAction(ContextMenuOptions contextMenu, ItemDisplay itemDisplay, Item item, bool isCurrency) {
			TrySendToStash(itemDisplay, CharacterManager.Instance.GetWorldHostCharacter());
		}

		private bool IsHost() {
			return Global.Lobby.IsWorldOwner;
		}
	}
}