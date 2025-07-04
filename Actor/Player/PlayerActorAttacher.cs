using System;
using parent_house_framework.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace gnomes.Actor.Player {
    public class PlayerActorAttacher : MonoBehaviour {
        [SerializeField, FoldoutGroup("Settings")]
        private ActorDetails DummyDetails;
        
        [SerializeField, FoldoutGroup("Settings")]
        private bool killOnRelease = false;

        private void OnEnable() {
            PlayerManager.AddPlayer += HandleAddPlayer;
            PlayerManager.RemovePlayer += HandleRemovePlayer;
        }

        private void OnDisable() {
            PlayerManager.AddPlayer -= HandleAddPlayer;
            PlayerManager.RemovePlayer -= HandleRemovePlayer;
        }

        private void HandleAddPlayer(Guid playerId) {
            var player = PlayerManager.Players[playerId];
            if (player.TryGetComponent(out Gnome spawnedActor)) {
                spawnedActor.Possess(player.GetId());
            }
        }
        
        private void HandleRemovePlayer(Guid playerId) {
            var player = PlayerManager.Players[playerId];
            if (player.TryGetComponent(out Gnome spawnedActor)) {
                spawnedActor.Possess(player.GetId());
            }
        }
    }
}