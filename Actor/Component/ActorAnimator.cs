using Sirenix.OdinInspector;
using UnityEngine;

namespace Gnomes.Actor.Component {
    /// <summary>
    /// Animator Todo:
    /// 1.) Figure out how to integrate ActorAnimationBehavior
    /// |-> ? Possible solution ?
    /// |---> 1.) Use conditions to find matching animations
    /// |---> 2.) If multiple, sort by a priority
    /// |---> 3.) Play first animation
    /// |---> NOTE: This requires a class from an outside package (parent-house-framework)
    /// </summary>
    
    [RequireComponent(typeof(Actor))]
    public class ActorAnimator : ActorComponent {
        [SerializeField, FoldoutGroup("Settings")]
        private float TurnSpeed;
    
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

        private static readonly int IsMovingAnimationId = Animator.StringToHash("IsMoving");
        private static readonly int DeathTriggerAnimationId = Animator.StringToHash("Death");

        protected override void OnEnable() {
            base.OnEnable();
            FetchDependencies();
            CurrentTargetScale = Vector3.one;
            CachedScale = transform.localScale;
        }
    
        private void FetchDependencies() {
            if (Anim == null) Anim = GetComponentInChildren<Animator>();

            if (TryGetComponent(out ActorMotor motor)) {
                Motor = motor;
            }
        }

        private void FixedUpdate() {
            if (!Motor) return;
            HandleMoveAnimations();
            HandleTurnAnimations();
        }

        private void HandleMoveAnimations() {
            // var isMoving = (Vector3.Distance(transform.position, Motor.Agent.destination) > 0.2f);
            var isMoving = Motor.Agent.velocity.sqrMagnitude > 0.2f;
            Anim.SetBool(IsMovingAnimationId, isMoving);
        }

        private void HandleTurnAnimations() {
            CurrentTargetScale.x = Motor.FacingDirection * CachedScale.x;
            CurrentTargetScale.y = CachedScale.y;
            CurrentTargetScale.z = CachedScale.z;
            if (ScaledTransform.localScale == CurrentTargetScale) return;
            ScaledTransform.localScale = Vector3.Lerp(ScaledTransform.localScale, CurrentTargetScale, Time.deltaTime * TurnSpeed);
        }

        protected override void HandleActorChanged(ActorDetails newDetails) {
            // Todo: Handle character change on Animator
        }
    
        protected override void HandleActorDeath() {
            base.HandleActorDeath();
            Anim.SetTrigger(DeathTriggerAnimationId);
        }

        [Button]
        public void SetTrigger(string trigger) {
            Anim.SetTrigger(trigger);
        }
    }
}