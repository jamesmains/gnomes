using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using parent_house_framework.Managed;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace gnomes.Actor.Player {
    public class PlayerControllerBase : SerializedManagedGameObject {
        
        [SerializeField, FoldoutGroup("Status"),ReadOnly]
        public int DeviceId;
        
        private InputUser inputUser;
        private bool IsOwner(int deviceId) => deviceId == PlayerManager.Players[GetId()].DeviceId;
        private List<(InputAction, Action<InputAction.CallbackContext>)> registeredCallbacks = new();
        private Coroutine WaitForReturnRoutine;
        private const float RejoinTime = 5f;

        public virtual void RegisterInputSystem(InputDevice device, InputActionAsset actionsAsset) {
            InputControlScheme? matchedScheme = null;
            registeredCallbacks = new();
            
            foreach (var scheme in actionsAsset.controlSchemes) {
                if (scheme.SupportsDevice(device)) {
                    matchedScheme = scheme;
                    break;
                }
            }

            if (matchedScheme == null) {
                Debug.LogError($"No matching control schemes for {device.name}");
                return;
            }

            DeviceId = device.deviceId;

            var inputActions = Instantiate(actionsAsset);
            inputUser = InputUser.CreateUserWithoutPairedDevices();
            inputUser.AssociateActionsWithUser(inputActions);
            inputUser.ActivateControlScheme(matchedScheme.Value.name);
            inputActions.Enable();
            InputUser.PerformPairingWithDevice(device, inputUser);
            InputUser.onChange += HandleUserChange;

            var actionMap = inputActions.FindActionMap("Player");

            foreach (var action in actionMap.actions) {
                string methodName = action.name;
                MethodInfo method = GetType().GetMethod(methodName,
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                if (method != null) {
                    Action<InputAction.CallbackContext> callback = (ctx) => method.Invoke(this, new object[] { ctx });

                    action.performed += callback;
                    if (action.type == InputActionType.Value)
                        action.canceled += callback;
                    if(registeredCallbacks == null) {
                        Debug.Log("Registered callbacks is null!");
                        return;
                    }
                    registeredCallbacks.Add((action, callback));
                }
                else {
                    Debug.LogWarning($"No method found for action '{methodName}' on {name}");
                }
            }
        }

        public virtual void UnregisterInputSystem() {
            foreach (var (action, callback) in registeredCallbacks) {
                action.performed -= callback;
                if (action.type == InputActionType.Value)
                    action.canceled -= callback;
            }

            InputUser.onChange -= HandleUserChange;
            inputUser.UnpairDevicesAndRemoveUser();
            registeredCallbacks.Clear();
        }

        private void HandleUserChange(InputUser user, InputUserChange change, InputDevice device) {
            if (device == null) return;
            if (!IsOwner(device.deviceId)) return;
            switch (change) {
                case InputUserChange.DeviceLost:
                    Debug.LogWarning($"[Player {user.index}] Device lost: {device.displayName}");
                    HandleDeviceLost(user, device);
                    break;

                case InputUserChange.DeviceRegained:
                    Debug.Log($"[Player {user.index}] Device regained: {device.displayName}");
                    HandleDeviceRegained(user, device);
                    break;

                // Optional: detect other relevant changes
                case InputUserChange.ControlSchemeChanged:
                    Debug.Log($"[Player {user.index}] Control scheme changed to {user.controlScheme?.name}");
                    break;
            }
        }

        // Note: this basically doesn't work atm, at least not with the devices I can test it never hits DeviceRegained
        private void HandleDeviceRegained(InputUser user, InputDevice device) {
            Debug.Log("Trying to stop coroutine");
            if (WaitForReturnRoutine != null) {
                Debug.Log("Stopped coroutine");
                StopCoroutine(WaitForReturnRoutine);
            }
        }

        private void HandleDeviceLost(InputUser user, InputDevice device) {
            WaitForReturnRoutine = StartCoroutine(WaitForUserToReturn());
        }

        private IEnumerator WaitForUserToReturn() {
            var time = 0f;
            while (time < RejoinTime) {
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            HandleLeave();
            PlayerManager.TryLeave.Invoke(GetId());
        }

        protected virtual void Move(InputAction.CallbackContext ctx) { }

        protected virtual void HandleLeave() { }

        protected virtual void PrimaryAction(InputAction.CallbackContext ctx) { }

        protected virtual void SecondaryAction(InputAction.CallbackContext ctx) { }

        protected virtual void Interact(InputAction.CallbackContext ctx) { }

        protected virtual void Confirm(InputAction.CallbackContext ctx) { }

        protected virtual void Cancel(InputAction.CallbackContext ctx) {
            // Todo: Figure out solution for this debug setup
            PlayerManager.TryLeave.Invoke(GetId());
        }
    }
}