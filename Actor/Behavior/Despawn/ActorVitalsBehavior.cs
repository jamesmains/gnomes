using System;
using parent_house_framework.Utils;
using parent_house_framework.Values;
using Sirenix.OdinInspector;
using UnityEngine;

namespace gnomes.Actor.Behavior.Despawn {
    [Serializable]
    public class ActorVitalsBehavior: ActorBehavior {

        [SerializeField, FoldoutGroup("Status")]
        private float TimeTillDespawn;
        
        [SerializeField, FoldoutGroup("Status")]
        private bool IsAlive;
        
        public override ActorBehavior Create<T>(Actor owner) {
            return new ActorVitalsBehavior(owner);
        }

        public ActorVitalsBehavior(Actor ownerActor) : base(ownerActor) {
            OwnerActor.Bindings.TryAdd(OwnerActor.GetId(), new BindingBool("isAlive",ref IsAlive));
        }

        public ActorVitalsBehavior() { }

        #region Methods

        public override void Update() {
            if (IsAlive || !(Time.time > TimeTillDespawn)) return;
            
            if (OwnerActor.Details?.DespawnEffect) {
                Pooler.SpawnAt(OwnerActor.Details.DespawnEffect, OwnerActor.transform.position);
            }
            OwnerActor.Despawn();
        }

        public override void FixedUpdate() {
        }
        
        public virtual void HandleActorLifeStateChange(bool isAlive) {
            Debug.Log("HandleActorLifeStateChange: "+isAlive);
            IsAlive = OwnerActor.Bindings[OwnerActor.GetId()].Value = isAlive;
            if(!IsAlive)
                TimeTillDespawn = OwnerActor.Details?.DespawnTimer + Time.time ?? 1f + Time.time;
        }

        #endregion
    }
}