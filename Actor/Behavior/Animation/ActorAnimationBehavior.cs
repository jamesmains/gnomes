using System;
using Gnomes.Actor.Component;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gnomes.Actor.Behavior.Animation {
    /// <summary>
    /// Actor Behavior Collection
    /// Animators / Animation Controllers
    /// Todo:
    /// </summary>
    [Serializable]
    public abstract class ActorAnimationBehavior : ActorBehavior
    {
        protected ActorAnimationBehavior(Actor ownerActor) : base(ownerActor) {
        }

        protected ActorAnimationBehavior() {
            
        }
        // Typically you may want to use this with an Animator, or set up your behaviors to utilize strings.
        public virtual void PlayAnimationByName(string animationName) {
        
        }
        public virtual void PlayAnimationById(int animationId) {
        
        }
    }

    /// <summary>
    /// Basic Animation System
    /// Todo: Find more suitable name
    /// </summary>
    [Serializable]
    public class ActorAnimation : ActorAnimationBehavior {
        [SerializeField, FoldoutGroup("Settings")]
        private float TurnSpeed = 5;
    
        [SerializeField, FoldoutGroup("Dependencies")]
        private Transform ScaledTransform;
        
        [SerializeField, FoldoutGroup("Dependencies"), ReadOnly]
        private Animator Anim;

        [SerializeField, FoldoutGroup("Dependencies"), ReadOnly]
        private ActorMotor Motor;
        
        [SerializeField, FoldoutGroup("Status"), ReadOnly]
        private Vector3 CachedScale;
    
        [SerializeField, FoldoutGroup("Status"), ReadOnly]
        private Vector3 CurrentTargetScale;
        
        public override ActorBehavior Create<T>(Actor owner) {
            return new ActorAnimation(owner);
        }
        public ActorAnimation(Actor ownerActor) : base(ownerActor) {
            var animators = ownerActor.GetComponentsInChildren<Animator>();
            BehaviorWarningMessage = animators.Length switch {
                > 1 => "Multiple animators found. This behavior relies on a single animator.",
                0 => "No animators found. This behavior relies on a single animator.",
                _ => String.Empty
            };
            Anim = animators[0];
            Motor = OwnerActor.GetComponent<ActorMotor>();
            ScaledTransform = Anim.transform;
            CurrentTargetScale = Vector3.one;
            CachedScale = ScaledTransform.transform.localScale;
        }

        public ActorAnimation() {
            
        }

        public override void PlayAnimationByName(string animationName) {
            base.PlayAnimationByName(animationName);
            Anim.SetTrigger(animationName);
        }
        
        public override void PlayAnimationById(int animationId) {
            base.PlayAnimationById(animationId);
            Anim.SetTrigger(animationId);
        }

        public override void Update() {
        }

        public override void FixedUpdate() {
            HandleMoveAnimations();
            HandleTurnAnimations();
        }
        
        private void HandleMoveAnimations() {
            Anim.SetBool(ActorAnimator.IsMovingAnimationId, Motor.IsMoving);
        }

        private void HandleTurnAnimations() {
            // Todo: Move to animation controller?
            CurrentTargetScale.x = Motor.FacingDirection * CachedScale.x;
            CurrentTargetScale.y = CachedScale.y;
            CurrentTargetScale.z = CachedScale.z;
            if (ScaledTransform.localScale == CurrentTargetScale) return;
            ScaledTransform.localScale = Vector3.Lerp(ScaledTransform.localScale, CurrentTargetScale, Time.deltaTime * TurnSpeed);
        }
    }
}