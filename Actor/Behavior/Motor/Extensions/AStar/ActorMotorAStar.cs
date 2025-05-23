using System;
using Gnomes.Actor.Component;
using Pathfinding;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gnomes.Actor.Behavior.Motor.Extensions.AStar {
    [Serializable]
    public class ActorMotorAStar : ActorMotorBehavior {
        [SerializeField, FoldoutGroup("Dependencies"), ReadOnly]
        public FollowerEntity Agent;
    
        [SerializeField, FoldoutGroup("Dependencies"), ReadOnly]
        public ActorMotor Motor;
    
        [SerializeField, FoldoutGroup("Status"), ReadOnly]
        private float CachedStopDistance;
    
        public override ActorBehavior Create<T>(Actor owner) {
            return new ActorMotorAStar(owner);
        }

        public ActorMotorAStar(Actor ownerActor) : base(ownerActor) {
            OwnerActor.TryGetComponent(out FollowerEntity agent);
            if (agent) {
                Agent = agent;
                BehaviorWarningMessage = String.Empty;
            }
            else
                BehaviorWarningMessage =
                    "There is no Follower Entity found on this actor. One is required for this behavior to function.";
            Motor = OwnerActor.GetComponent<ActorMotor>();
        }

        public ActorMotorAStar() {
        
        }

        public override void Update() {
        }

        public override void FixedUpdate() {
            Motor.IsMoving = Agent.velocity.sqrMagnitude > 0.2f;
        }

        public override void Move(Vector3 moveTarget, bool asDirection) {
            var targetPosition = asDirection ? Agent.transform.position + moveTarget: moveTarget;
            if (Agent.destination != targetPosition) {
                Agent.SetDestination(targetPosition);
                if(moveTarget.x != 0)
                    Motor.FacingDirection = moveTarget.x > 0 ? -1 : 1;
            }
        }

        public override void HandleActorChanged(ActorDetails actorDetails) {
            CachedStopDistance = OwnerActor.CurrentStopDistance;
            Agent.stopDistance = OwnerActor.Possessed ? 0 : CachedStopDistance;
        }

        public override void HandlePossession(Actor actor) {
            Agent.stopDistance = 0f;
        }

        public override void HandleReleasePossession(Actor actor) {
            Agent.stopDistance = CachedStopDistance;
        }
    }
}