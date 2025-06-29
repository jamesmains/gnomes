using System;
using gnomes.Actor.Component;
using Sirenix.OdinInspector;
using UnityEngine;

namespace gnomes.Actor.Behavior.Motor {
    [Serializable]
    public class ActorMotor2dRigidbody : ActorMotorBehavior {

        #region Properties

        [SerializeField, FoldoutGroup("Settings")]
        private LayerMask _groundLayerMask;
        
        [SerializeField, FoldoutGroup("Settings")]
        private float moveSpeed;
        
        [SerializeField, FoldoutGroup("Settings")]
        private float jumpForce;
        
        [SerializeField, FoldoutGroup("Dependencies"), ReadOnly]
        private Rigidbody2D _rb;
        
        [SerializeField, FoldoutGroup("Status"), ReadOnly]
        private bool _tryJumpNextFrame = false;
        
        [SerializeField, FoldoutGroup("Status"), ReadOnly]
        private Vector3 _moveDirection;

        #endregion

        #region Factory Methods and Constructors

        public override ActorBehavior Create<T>(Actor owner) {
            var behavior = new ActorMotor2dRigidbody(owner) {
                moveSpeed = moveSpeed,
                jumpForce = jumpForce,
                _groundLayerMask = _groundLayerMask,
            };
            return behavior;
        }

        public ActorMotor2dRigidbody(Actor owner) : base(owner) {
            // Try to get needed Rigidbody component
            if (OwnerActor.TryGetComponent(out Rigidbody2D rb)) {
                _rb = rb;
                BehaviorWarningMessage = String.Empty;
            }
            else
                BehaviorWarningMessage =
                    "There is no Rigidbody found on this actor. One is required for this behavior to function.";
            
            Motor = OwnerActor.GetComponent<ActorMotor>();
        }

        // Empty constructor so it's picked up by Odin Inspector
        public ActorMotor2dRigidbody() { }

        #endregion

        #region Methods

        public override void Update() { }

        public override void FixedUpdate() {
            _rb.AddForce(_moveDirection * moveSpeed);
            if (!_tryJumpNextFrame) return;
            _tryJumpNextFrame = false;
            _rb.AddForce(new Vector2(0, jumpForce));
        }

        public override void Move(Vector3 moveTarget) {
            if (_rb.IsTouchingLayers(_groundLayerMask) && moveTarget.y > 0) {
                _tryJumpNextFrame = true;
            }

            moveTarget.y = 0;
            _moveDirection = moveTarget;
            _moveDirection.z = 0;
        }

        #endregion
        
        
    }
}
