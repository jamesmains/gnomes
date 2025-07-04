using System;
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

        #region Methods

        public override void Init(Gnome parentGnome) {
            base.Init(parentGnome);
            
            // Try to get needed Rigidbody component
            if (ParentGnome.TryGetComponent(out Rigidbody2D rb)) {
                _rb = rb;
                BehaviorWarningMessage = String.Empty;
            }
            else
                BehaviorWarningMessage =
                    "There is no Rigidbody found on this actor. One is required for this behavior to function.";

        }

        public override void Update() { }

        public override void FixedUpdate() {
            _rb.AddForce(_moveDirection * moveSpeed);
            if (!_tryJumpNextFrame) return;
            _tryJumpNextFrame = false;
            _rb.AddForce(new Vector2(0, jumpForce));
        }

        public override void HandleMove(Vector2 moveTarget) {
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
