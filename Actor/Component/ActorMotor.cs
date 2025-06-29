using System;
using gnomes.Actor.Behavior.Motor;
using Sirenix.OdinInspector;
using UnityEngine;

namespace gnomes.Actor.Component {
    public class ActorMotor : ActorComponent
    {
        [SerializeField, FoldoutGroup("Settings")]
        private ActorMotorBehavior MotorBehavior;

        [SerializeField, FoldoutGroup("Status"), ReadOnly]
        public int FacingDirection; // -1 left, 1 right

        [SerializeField, FoldoutGroup("Status"), ReadOnly]
        public bool IsMoving;

        protected override void OnEnable() {
            base.OnEnable();
            FacingDirection = 1;
            FetchDependencies();
        }

        private void FetchDependencies() {
            MotorBehavior = MotorBehavior.Create<ActorMotorBehavior>(this.Actor) as ActorMotorBehavior;
        }

        private void Update() {
            MotorBehavior?.Update();
        }

        private void FixedUpdate() {
            MotorBehavior?.FixedUpdate();
        }

        protected override void HandleMove(Vector3 targetVector) {
            if (!Actor.IsActive()) return;
            MotorBehavior?.Move(targetVector);
        }

        protected override void HandleActorChanged(ActorDetails actorDetails) {
            MotorBehavior?.HandleActorChanged(actorDetails);
        }

        protected override void HandlePossession(Guid ownerId) {
            MotorBehavior?.HandlePossession(ownerId);
        }

        protected override void HandleReleasePossession(Guid ownerId) {
            MotorBehavior?.HandleReleasePossession(ownerId);
        }
    }
}
