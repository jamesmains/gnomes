using System;
using UnityEngine;

namespace Gnomes.Actor.Behavior.Motor {
    [Serializable]
    public abstract class ActorMotorBehavior : ActorBehavior
    {
        protected ActorMotorBehavior(Actor ownerActor) : base(ownerActor) {
            OwnerActor = ownerActor;
        }
        
        protected ActorMotorBehavior() {}
    
        public virtual void Move(Vector3 moveTarget, bool asDirection){}
        public virtual void HandleActorChanged(ActorDetails actorDetails) {}
        public virtual void HandlePossession(Actor actor) {}
        public virtual void HandleReleasePossession(Actor actor) {}
    }
}