using System;
using gnomes.Actor.Component;
using Sirenix.OdinInspector;
using UnityEngine;

namespace gnomes.Actor.Behavior.Motor {
    [Serializable]
    public class ActorMotor3dRigidbody : ActorMotorBehavior {

        #region Properties

        [SerializeField, FoldoutGroup("Settings")]
        private float moveSpeed;
        
        [SerializeField, FoldoutGroup("Dependencies"), ReadOnly]
        private Rigidbody _rb;
        
        [SerializeField, FoldoutGroup("Status"), ReadOnly]
        private Vector3 _moveDirection;

        #endregion

        #region Factory Methods and Constructors

        public override ActorBehavior Create<T>(Actor owner) {
            var behavior = new ActorMotor3dRigidbody(owner) {
                moveSpeed = moveSpeed
            };
            return behavior;
        }

        public ActorMotor3dRigidbody(Actor owner) : base(owner) {
            // Try to get needed Rigidbody component
            if (OwnerActor.TryGetComponent(out Rigidbody rb)) {
                _rb = rb;
                BehaviorWarningMessage = String.Empty;
            }
            else
                BehaviorWarningMessage =
                    "There is no Rigidbody found on this actor. One is required for this behavior to function.";
            
            Motor = OwnerActor.GetComponent<ActorMotor>();
        }

        // Empty constructor so it's picked up by Odin Inspector
        public ActorMotor3dRigidbody() { }

        #endregion

        #region Methods

        public override void Update() { }

        public override void FixedUpdate() {
            _rb.AddForce(_moveDirection * moveSpeed);
        }

        public override void Move(Vector3 moveTarget) {
            _moveDirection.z = _moveDirection.y;
            _moveDirection.y = 0;
            _moveDirection = moveTarget;
        }

        #endregion
        
        
    }
}
