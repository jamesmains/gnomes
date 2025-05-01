using System;
using Gnomes.Actor.Behavior.Animation;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gnomes.Actor.Component {
    
    [RequireComponent(typeof(Actor))]
    public class ActorAnimator : ActorComponent {
        public static readonly int IsMovingAnimationId = Animator.StringToHash("IsMoving");
        public static readonly int DeathTriggerAnimationId = Animator.StringToHash("Death");
        
        [SerializeField, FoldoutGroup("Settings")]
        private ActorAnimationBehavior AnimationBehavior;
        protected override void OnEnable() {
            base.OnEnable();
            FetchDependencies();
        }
    
        private void FetchDependencies() {
            AnimationBehavior = AnimationBehavior.Create<ActorAnimationBehavior>(this.Actor) as ActorAnimationBehavior;
        }

        private void Update() {
            AnimationBehavior?.Update();
        }

        private void FixedUpdate() {
            AnimationBehavior?.FixedUpdate();
        }
        
        protected override void HandleActorChanged(ActorDetails newDetails) {
            // Todo: Handle character change on Animator
        }
    
        protected override void HandleActorDeath() {
            AnimationBehavior.PlayAnimationById(DeathTriggerAnimationId);
        }
    }
}