
using System;
using Gnomes.Actor.Behavior.Motor;
using Sirenix.OdinInspector;
using UnityEngine;

// Todo: Maybe find a new nav agent component
namespace Gnomes.Actor.Component {
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
            MotorBehavior.Update();
        }

        private void FixedUpdate() {
            MotorBehavior.FixedUpdate();
        }

        protected override void HandleMove(Vector3 targetVector, bool asDirection) {
            if (Actor.Dead) return;
            MotorBehavior.Move(targetVector, asDirection);
        }

        protected override void HandleActorChanged(ActorDetails actorDetails) {
            MotorBehavior.HandleActorChanged(actorDetails);
        }

        protected override void HandlePossession(Actor actor) {
            if (actor != Actor) return;
            MotorBehavior.HandlePossession(actor);
        }

        protected override void HandleReleasePossession(Actor actor) {
            if (actor != Actor) return;
            MotorBehavior.HandleReleasePossession(actor);
        }
    }
}
