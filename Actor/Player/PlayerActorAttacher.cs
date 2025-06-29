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
        
        [SerializeField, FoldoutGroup("Settings")]
        private GameObject ActorPrefab;

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
            var actorObj = Pooler.SpawnAt(ActorPrefab, player.PlayerController.transform.position);
            if (actorObj.TryGetComponent(out Actor spawnedActor)) {
                player.PlayerController.PossessActor(spawnedActor);
                spawnedActor.Spawn();
            }
        }
        
        private void HandleRemovePlayer(Guid playerId) {
            var player = PlayerManager.Players[playerId];
            player.PlayerController.ReleasePossession(killOnRelease);
        }
    }
}