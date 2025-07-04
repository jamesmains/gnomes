using System;
using parent_house_framework.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace gnomes.Actor.Behavior.Vitals {
    [Serializable]
    public class ActorVitalsBehavior: ActorBehavior {

        [SerializeField, FoldoutGroup("Status")]
        private float TimeTillDespawn;
        
        [SerializeField, FoldoutGroup("Status")]
        private bool IsAlive;
        
        #region Methods

        public override void Update() {
            if (IsAlive || !(Time.time > TimeTillDespawn)) return;
            
            if (ParentGnome.Details?.DespawnEffect) {
                Pooler.SpawnAt(ParentGnome.Details.DespawnEffect, ParentGnome.transform.position);
            }
            // ParentGnome.Despawn();
        }

        public override void FixedUpdate() {
        }
        
        public virtual void HandleActorLifeStateChange(bool isAlive) {
            Debug.Log("HandleActorLifeStateChange: "+isAlive);
            IsAlive = isAlive;
            if(!IsAlive)
                TimeTillDespawn = ParentGnome.Details?.DespawnTimer + Time.time ?? 1f + Time.time;
        }

        #endregion
    }
}