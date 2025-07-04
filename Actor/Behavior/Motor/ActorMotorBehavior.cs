using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace gnomes.Actor.Behavior.Motor {
    [Serializable]
    public abstract class ActorMotorBehavior : ActorBehavior {
        private bool isGrounded;

        public bool IsGrounded {
            get => isGrounded;
            set {
                if (value == IsGroundedCache) return;
                IsGroundedCache = isGrounded = value;
                ParentGnome.OnSetAnimationBool.Invoke("Jumped", !isGrounded);
                ParentGnome.OnSetAnimationBool.Invoke("Landed", isGrounded);
            }
        }

        [SerializeField, FoldoutGroup("Status"), ReadOnly]
        public bool IsGroundedCache;

        public virtual Vector2 GetVelocity() {
            return Vector2.zero;
        }

        public virtual void BroadcastVelocity(Vector2 velocity) {
            ParentGnome.OnSetAnimationFloat("X", velocity.x);
            ParentGnome.OnSetAnimationFloat("Y", velocity.y);
        }
    }
}