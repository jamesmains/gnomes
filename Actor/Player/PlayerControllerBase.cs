using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using parent_house_framework.Managed;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace gnomes.Actor.Player {
    public class PlayerControllerBase : ManagedGameObject {
        private InputUser inputUser;
        private List<(InputAction, Action<InputAction.CallbackContext>)> registeredCallbacks = new();
        private Actor CurrentActor;
        private bool IsOwner(int deviceId) => deviceId == PlayerManager.Players[GetId()].DeviceId;
        private Coroutine WaitForReturnRoutine;
        private const float RejoinTime = 5f;

        public void RegisterInputSystem(InputDevice device, InputActionAsset actionsAsset) {
            InputControlScheme? matchedScheme = null;

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

                    registeredCallbacks.Add((action, callback));
                }
                else {
                    Debug.LogWarning($"No method found for action '{methodName}' on {name}");
                }
            }
        }

        public void UnregisterInputSystem() {
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
            CurrentActor?.OnReleasePossession.Invoke(GetId());
            PlayerManager.TryLeave.Invoke(GetId());
        }

        private void Move(InputAction.CallbackContext ctx) {
            Vector2 input = ctx.ReadValue<Vector2>();
            // callbackContext.ReadValue<Vector2>();
            CurrentActor?.OnMove?.Invoke(input);
        }

        private void PrimaryAction(InputAction.CallbackContext ctx) {
            CurrentActor?.OnUse?.Invoke();
        }

        private void SecondaryAction(InputAction.CallbackContext ctx) {
            CurrentActor?.OnInteract?.Invoke();
        }

        private void Interact(InputAction.CallbackContext ctx) {
        }

        private void Confirm(InputAction.CallbackContext ctx) {
        }

        private void Cancel(InputAction.CallbackContext ctx) {
            PlayerManager.TryLeave.Invoke(GetId());
        }

        /// <summary>
        /// Attaches the player's input to the Actor
        /// </summary>
        /// <param name="actor">Target actor to control</param>
        public void PossessActor(Actor actor) {
            CurrentActor = actor;
            CurrentActor?.OnPossessed?.Invoke(GetId());
        }

        /// <summary>
        /// Releases control of the actor from the player
        /// </summary>
        public void ReleasePossession(bool killOnRelease = false) {
            if (killOnRelease)
                CurrentActor?.KillActor();
            CurrentActor?.OnReleasePossession?.Invoke(GetId());
            CurrentActor = null;
        }
    }
}