using System;
using gnomes.Actor.Component;
using Sirenix.OdinInspector;
using UnityEngine;

namespace gnomes.Actor.Behavior.Motor {
    [Serializable]
    public class ActorMotor2dTopDownRigidbody: ActorMotorBehavior  {
        
        #region Properties
        
        [SerializeField, FoldoutGroup("Settings")]
        private float moveSpeed;
        
        [SerializeField, FoldoutGroup("Dependencies"), ReadOnly]
        private Rigidbody2D _rb;
        
        [SerializeField, FoldoutGroup("Status"), ReadOnly]
        private Vector3 _moveDirection;

        #endregion

        #region Factory Methods and Constructors

        public override ActorBehavior Create<T>(Actor owner) {
            var behavior = new ActorMotor2dTopDownRigidbody(owner) {
                moveSpeed = moveSpeed,
            };
            return behavior;
        }

        public ActorMotor2dTopDownRigidbody(Actor owner) : base(owner) {
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
        public ActorMotor2dTopDownRigidbody() { }

        #endregion

        #region Methods

        public override void Update() { }

        public override void FixedUpdate() {
            _rb.AddForce(_moveDirection * moveSpeed);
        }

        public override void Move(Vector3 moveTarget) {
            Debug.Log(moveTarget);
            _moveDirection = moveTarget;
            _moveDirection.z = 0;
        }

        #endregion
        
        
    }
}