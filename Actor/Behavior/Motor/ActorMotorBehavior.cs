using System;
using gnomes.Actor.Component;
using Sirenix.OdinInspector;
using UnityEngine;

namespace gnomes.Actor.Behavior.Motor {
    [Serializable]
    public abstract class ActorMotorBehavior : ActorBehavior
    {
        [SerializeField, FoldoutGroup("Dependencies"), ReadOnly]
        public ActorMotor Motor;
        
        protected ActorMotorBehavior(Actor ownerActor) : base(ownerActor) {
            OwnerActor = ownerActor;
        }
        
        protected ActorMotorBehavior() {}
    
        public virtual void Move(Vector3 moveTarget){}
        public virtual void HandleActorChanged(ActorDetails actorDetails) {}
        public virtual void HandlePossession(Guid ownerId) {}
        public virtual void HandleReleasePossession(Guid ownerId) {}
    }
}