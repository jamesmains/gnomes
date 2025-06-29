using System;
using parent_house_framework.Managed;
using Sirenix.OdinInspector;
using UnityEngine;

namespace gnomes.Actor.Component {
    public class ActorComponent: SerializedManagedGameObject {
    
        [SerializeField, FoldoutGroup("Status"),ReadOnly]
        public Actor Actor;

        protected override void OnEnable() {
            base.OnEnable();
            if(!Actor) Actor = GetComponent<Actor>();
            Actor.OnMove += HandleMove;
            Actor.OnPossessed += HandlePossession;
            Actor.OnReleasePossession += HandleReleasePossession;
            Actor.OnActorSet += HandleActorChanged;
            Actor.OnLifeStateChange += HandleActorLifeStateChange;
        }
    
        protected override void OnDisable() {
            base.OnDisable();
            Actor.OnMove -= HandleMove;
            Actor.OnPossessed -= HandlePossession;
            Actor.OnReleasePossession -= HandleReleasePossession;
            Actor.OnActorSet -= HandleActorChanged;
            Actor.OnLifeStateChange -= HandleActorLifeStateChange;
        }
    
        protected virtual void HandleActorChanged(ActorDetails newDetails) {
        
        }

        protected virtual void HandleActorLifeStateChange(bool isAlive) {
        
        }

        protected virtual void HandlePossession(Guid ownerId) {
            
        }

        protected virtual void HandleReleasePossession(Guid ownerId) {
            
        }

        protected virtual void HandleMove(Vector3 targetVector) {
            
        }
    }
}