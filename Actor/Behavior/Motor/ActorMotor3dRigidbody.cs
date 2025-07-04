using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace gnomes.Actor.Behavior.Motor {
    [Serializable]
    public class ActorMotor3dRigidbody : ActorMotorBehavior {
        #region Properties

        [SerializeField, FoldoutGroup("Settings")]
        private Vector2 movementRampUpSpeed;

        [SerializeField, FoldoutGroup("Settings")]
        private LayerMask groundLayer;

        [SerializeField, FoldoutGroup("Settings")]
        private float gravity;

        [SerializeField, FoldoutGroup("Settings")]
        private float moveSpeed;

        [SerializeField, FoldoutGroup("Settings")]
        private float jumpForce;

        [SerializeField, FoldoutGroup("Settings")]
        private float rotationSpeed;

        [SerializeField, FoldoutGroup("Dependencies"), ReadOnly]
        private Rigidbody _rb;

        [SerializeField, FoldoutGroup("Status"), ReadOnly]
        private bool jumpQueued;

        [SerializeField, FoldoutGroup("Status"), ReadOnly]
        private Vector3 moveInput;

        [SerializeField, FoldoutGroup("Status"), ReadOnly]
        private Vector3 targetMoveInput;

        [SerializeField, FoldoutGroup("Status"), ReadOnly]
        private Vector3 lastPosition;

        [SerializeField, FoldoutGroup("Status"), ReadOnly]
        private Vector3 velocity;

        [SerializeField, FoldoutGroup("Status"), ReadOnly]
        private Camera mainCamera;

        #endregion
        
        #region Methods

        public override void Init(Gnome parentGnome) {
            base.Init(parentGnome);
            // Try to get needed Rigidbody component
            if (ParentGnome.TryGetComponent(out Rigidbody rb)) {
                _rb = rb;
                BehaviorWarningMessage = String.Empty;
            }
            else
                BehaviorWarningMessage =
                    "There is no Rigidbody found on this actor. One is required for this behavior to function.";

            mainCamera = Camera.main;
        }

        public override void Update() { }

        public override void FixedUpdate() {
            IsGrounded = Physics.CheckSphere(ParentGnome.transform.position, 0.4f, groundLayer);
            if (!IsGrounded) {
                _rb.AddForce(ParentGnome.transform.up * gravity, ForceMode.Force);
            }

            if (jumpQueued) {
                jumpQueued = false;
                _rb.AddForce(ParentGnome.transform.up * jumpForce, ForceMode.Force);
            }

            velocity = (ParentGnome.transform.position - lastPosition) / Time.deltaTime;
            lastPosition = ParentGnome.transform.position;
            float rampSpeed = moveInput.magnitude > targetMoveInput.magnitude
                ? movementRampUpSpeed.x
                : movementRampUpSpeed.y;
            targetMoveInput = Vector3.Lerp(targetMoveInput, moveInput, Time.deltaTime * rampSpeed);
            if (targetMoveInput.sqrMagnitude > 0.01f) {
                Vector3 camForward = mainCamera.transform.forward;
                Vector3 camRight = mainCamera.transform.right;

                camForward.y = 0;
                camRight.y = 0;

                camForward.Normalize();
                camRight.Normalize();

                Vector3 move = (camForward * targetMoveInput.y + camRight * targetMoveInput.x);
                ParentGnome.transform.position += move * (moveSpeed * Time.deltaTime);
                if (move != Vector3.zero) {
                    Vector3 flatDirection = new Vector3(move.x, 0f, move.z);

                    if (flatDirection.sqrMagnitude > 0.001f) {
                        Quaternion targetRotation = Quaternion.LookRotation(flatDirection, Vector3.up);
                        ParentGnome.transform.rotation = Quaternion.Slerp(ParentGnome.transform.rotation, targetRotation,
                            rotationSpeed * Time.deltaTime);
                    }
                }
            }
            BroadcastVelocity(GetVelocity());
        }

        public override void HandleMove(Vector2 moveTarget) {
            moveInput = moveTarget;
        }

        public override void HandleJump() {
            if (IsGrounded)
                jumpQueued = true;
        }

        public override Vector2 GetVelocity() {
            Vector3 flatVelocity = new Vector3(velocity.x, 0f, velocity.z);
            float speed = flatVelocity.magnitude;
            float maxSpeed = moveSpeed;

            // Clamp speed to 1
            float blendSpeed = Mathf.Clamp01(speed / maxSpeed-0.15f);

            // Get direction in local space
            Vector3 localVelocity = ParentGnome.transform.InverseTransformDirection(flatVelocity).normalized;

            // Final blend tree vector
            float moveX = localVelocity.x * blendSpeed;
            float moveY = localVelocity.z * blendSpeed;
            return new Vector2(moveX, moveY);
        }

        #endregion
    }
}