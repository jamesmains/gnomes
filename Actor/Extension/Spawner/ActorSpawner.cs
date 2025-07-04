using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace gnomes.Actor.Extension.Spawner {
    public class ActorSpawner : gnomes.Spawner.Spawner {
        [SerializeField, FoldoutGroup("Settings")]
        private List<ActorDetails> ActorPool;
    
        [SerializeField, FoldoutGroup("Status"), ReadOnly]
        private int ActorSpawnIndex;

        protected override void Spawn() {
            base.Spawn();
            var spawnedActor = CurrentSpawningObject.GetComponent<Gnome>();
            spawnedActor.ChangeDetails(ActorPool[GetIndex(ref ActorSpawnIndex, ActorPool.Count)]);
        }
    }
}
