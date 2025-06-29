using System;
using gnomes.Interfaces;
using parent_house_framework.Managed;
using Sirenix.OdinInspector;
using UnityEngine;

// This is for more generic spawnables that don't really need to care about what happens when spawned.
namespace gnomes.Spawner {
    public class Spawnable : ManagedGameObject, ISpawnable {
        [SerializeField, FoldoutGroup("Status")]
        protected bool Spawned;

        public Action<ISpawnable> OnSpawn { get; set; }
        public Action<ISpawnable> OnDespawn { get; set; }

        protected override void OnDisable() {
            Despawn();
        }
        
        public void Spawn() {
            Spawned = true;
            OnSpawn.Invoke(this);
        }

        public void Despawn() {
            Spawned = false;
            OnDespawn?.Invoke(this);
            this.gameObject.SetActive(false);
        }
    }
}
