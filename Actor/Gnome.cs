using System;
using gnomes.Actor.Behavior;
using gnomes.Actor.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace gnomes.Actor {
    public class Gnome : PlayerControllerBase {

        public ActorDetails Details;
        public readonly ActorBehavior BrainBehavior;
        public readonly ActorBehavior AnimationBehavior;
        public readonly ActorBehavior VitalsBehavior;
        public readonly ActorBehavior MotorBehavior;
    
        private Guid ControllingPlayerId;
        public bool PlayerControlled() => ControllingPlayerId != Guid.Empty;

        // Actions
        [HideInInspector] public Action<Vector2> OnMove;
        [HideInInspector] public Action OnJump;
        [HideInInspector] public Action<Vector2> OnAim;
        [HideInInspector] public Action OnUse;
        [HideInInspector] public Action OnInteract;

        // Vitals
        [HideInInspector] public Action OnKill;
        [HideInInspector] public Action OnRevive;

        // Animations
        [HideInInspector] public Action<string> OnSetAnimationTrigger;
        [HideInInspector] public Action<string, bool> OnSetAnimationBool;
        [HideInInspector] public Action<string, int> OnSetAnimationInt;
        [HideInInspector] public Action<string, float> OnSetAnimationFloat;
    
        // Details
        [HideInInspector] public Action<ActorDetails> OnChangeDetails;
        
        private void Awake() {
            BrainBehavior?.Init(this);
            AnimationBehavior?.Init(this);
            VitalsBehavior?.Init(this);
            MotorBehavior?.Init(this);
        }

        private void Update() {
            BrainBehavior?.Update();
            AnimationBehavior?.Update();
            VitalsBehavior?.Update();
            MotorBehavior?.Update();
        }

        private void FixedUpdate() {
            BrainBehavior?.FixedUpdate();
            AnimationBehavior?.FixedUpdate();
            VitalsBehavior?.FixedUpdate();
            MotorBehavior?.FixedUpdate();
        }

        public void Possess(Guid playerId) {
            ControllingPlayerId = playerId;
        }

        public void ReleasePossession() {
            ControllingPlayerId = Guid.Empty;
        }

        public void ChangeDetails(ActorDetails newDetails) {
            Details = newDetails;
            OnChangeDetails.Invoke(Details);
        }

        protected override void Move(InputAction.CallbackContext ctx) {
            OnMove?.Invoke(ctx.ReadValue<Vector2>());
        }

        protected override void Confirm(InputAction.CallbackContext ctx) {
            OnJump?.Invoke();
        }
    }
}