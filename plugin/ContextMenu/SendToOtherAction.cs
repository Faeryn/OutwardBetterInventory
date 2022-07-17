using BetterInventory.Extensions;
using UnityEngine;

namespace BetterInventory.ContextMenu {
	public class SendToOtherAction : ItemContextMenuAction {
		private int playerID;

		public SendToOtherAction(int playerID) {
			this.playerID = playerID;
		}

		public override string GetText(ItemDisplayOptionPanel contextMenu) {
			return $"Send to {GetPlayerName()}";
		}
		
		private string GetPlayerName() {
			if (!IsValid()) {
				return "UNKNOWN_PLAYER"; // This should never happen but it's here just in case
			}
			return Global.Lobby.PlayersInLobby[playerID].Name;
		}

		private bool IsValid() {
			return Global.Lobby.PlayersInLobby.Count > playerID;
		}
		
		protected override bool IsActive(GameObject pointerPress, ItemDisplay itemDisplay, Item item) {
			return BetterInventory.SendToOtherEnabled.Value && IsValid() && !Global.Lobby.PlayersInLobby[playerID].IsLocalPlayer;
		}

		protected override void ExecuteAction(ItemDisplayOptionPanel contextMenu, ItemDisplay itemDisplay, Item item) {
			TrySendToOther(itemDisplay);
		}

		private void TrySendToOther(ItemDisplay itemDisplay) {
			if (!IsValid()) {
				return;
			}
			
			Character otherCharacter = Global.Lobby.PlayersInLobby[playerID].ControlledCharacter;
			if (otherCharacter.IsLocalPlayer) {
				return;
			}
			
			float maxDistanceSq = BetterInventory.SendToOtherMaxDistance.Value * BetterInventory.SendToOtherMaxDistance.Value;
			Character character = itemDisplay.m_characterUI.TargetCharacter;
			if (Vector3.SqrMagnitude(otherCharacter.transform.position - character.transform.position) > maxDistanceSq) {
				character.CharacterUI.ShowInfoNotification($"{otherCharacter.Name} is too far.");
				return;
			}

			ItemContainer targetContainer = otherCharacter.Inventory.Pouch;
			targetContainer.SetTemporaryDisplayName(otherCharacter.Name);
			try {
				itemDisplay.TryMoveTo(targetContainer);
			} finally {
				targetContainer.ClearTemporaryDisplayName();
			}
		}
	}
}