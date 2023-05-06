using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BetterInventory.ContextMenu {
	public class SendToStashAction : ItemContextMenuAction {
		// Scene, (QuestEvent id, false if it has to be missing)
		private static readonly Dictionary<string, StashInfo> StashInfoDict = new Dictionary<string, StashInfo>{
			{"CierzoNewTerrain", new StashInfo("-Ku9dHjTl0KeUPxWk0ZWWQ", "ImqRiGAT80aE2WtUHfdcMw", false)},
			{"Berg", new StashInfo("g403vlCU6EG0s1mI6t_rFA", "ImqRiGAT80aE2WtUHfdcMw", true)},
			{"Monsoon", new StashInfo("shhCMFa-lUqbIYS9hRcsdg", "ImqRiGAT80aE2WtUHfdcMw", true)},
			{"Levant", new StashInfo("LpVUuoxfhkaWOgh6XLbarA", "ZbPXNsPvlUeQVJRks3zBzg", true)},
			{"Harmattan", new StashInfo("0r087PIxTUqoj6N7z2HFNw", "ImqRiGAT80aE2WtUHfdcMw", true)},
			{"NewSirocco", new StashInfo(null, "IqUugGqBBkaOcQdRmhnMng", true)}
		};

		public override string GetText(ContextMenuOptions contextMenu) {
			return LocalizationManager.Instance.GetLoc($"{BetterInventory.GUID}.action.send_to_stash.text");
		}

		protected override bool IsActive(GameObject pointerPress, ItemDisplay itemDisplay, Item item, bool isCurrency) {
			return BetterInventory.SendToStashEnabled.Value && TryGetStash(out _) && (isCurrency || item.IsChildToPlayer);
		}

		protected override void ExecuteAction(ContextMenuOptions contextMenu, ItemDisplay itemDisplay, Item item, bool isCurrency) {
			TrySendToStash(itemDisplay, itemDisplay.m_characterUI.TargetCharacter);
		}

		/*private bool IsNearOwnedStash(Character character) { // TODO This could be useful for mod compatibility
			foreach (TreasureChest treasureChest in Object.FindObjectsOfType<TreasureChest>()) {
				if (treasureChest.SpecialType == ItemContainer.SpecialContainerTypes.Stash) {
					return true;
				}
			}
			return false;
		}*/

		protected bool TryGetStash(out StashInfo stashInfo) {
			Scene scene = SceneManager.GetActiveScene();
			string sceneName = scene.name;
			if (StashInfoDict.ContainsKey(sceneName)) {
				stashInfo = StashInfoDict[sceneName];
				return stashInfo.QuestEventID == null || QuestEventManager.Instance.HasQuestEvent(stashInfo.QuestEventID) == stashInfo.RequiredEventValue;
			}

			stashInfo = null;
			return false;
		}
		
		protected void TrySendToStash(ItemDisplay itemDisplay, Character stashOwner) {
			if (!TryGetStash(out StashInfo stashInfo)) {
				return;
			}
			
			ItemContainer stash;
				
			if (Utilities.IsVheosLegacyStashesActive()) {
				BetterInventory.Log.LogDebug("Sending to legacy stash");
				// We ignore stashOwner here - we can't access non-host stashes anyway
				stash = ItemManager.Instance.GetItem(stashInfo.ChestUID) as ItemContainer;
			} else {
				BetterInventory.Log.LogDebug("Sending to shared stash");
				stash = stashOwner.Stash;
			}

			if (stash == null) {
				BetterInventory.Log.LogError("Failed to find stash");
			} else {
				itemDisplay.TryMoveTo(stash);
			}
		}
	}

	public class StashInfo {
		public string QuestEventID { get; }
		public string ChestUID { get; }
		public bool RequiredEventValue { get; }

		public StashInfo(string questEventID, string chestUid, bool requiredEventValue) {
			QuestEventID = questEventID;
			ChestUID = chestUid;
			RequiredEventValue = requiredEventValue;
		}
	}
}