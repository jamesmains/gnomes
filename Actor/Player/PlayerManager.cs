using System;
using System.Collections.Generic;
using System.Linq;
using parent_house_framework.Managed;
using parent_house_framework.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace gnomes.Actor.Player {
    public class PlayerManager: ManagedGameObject {
        [SerializeField, FoldoutGroup("Settings")]
        private InputActionAsset ActionAsset;
        [SerializeField, FoldoutGroup("Settings")]
        private GameObject PlayerPrefab; 
        
        // playerId, deviceId
        public static readonly Dictionary<Guid,PlayerControllerBase> Players = new();
        public static InputSystem_Actions Input;

        public static Action<Guid> AddPlayer;
        public static Action<Guid> RemovePlayer;
        public static Action<Guid> TryLeave;
        
        protected override void OnEnable() {
            Input = new InputSystem_Actions();
            Input.Player.Confirm.performed += HandleNewPlayer;
            TryLeave += HandleRemovePlayer;
            Input.Enable();
        }

        protected override void OnDisable() {
            base.OnDisable();
            Input.Disable();
            Input = null;
        }

        private bool DeviceInUse(int deviceId) => Players.Any(o => o.Value.DeviceId == deviceId);

        private void HandleNewPlayer(InputAction.CallbackContext callbackContext) {
            var deviceId = callbackContext.control.device.deviceId;
            // Check if device already controls a player
            if (DeviceInUse(deviceId)) {
                Debug.Log("Player already exists");
                return;
            }
            
            var newPlayerObj = Pooler.Spawn(PlayerPrefab);
            
            // Requires prefab has a Player component
            if (newPlayerObj.TryGetComponent(out PlayerControllerBase player)) {
                
                // Assign the player to the deviceId
                player.DeviceId = deviceId;
                
                // Add player to dictionary
                Players.Add(player.GetId(), player);
                
                // Broadcast AddPlayer event
                AddPlayer.Invoke(player.GetId());
                
                // Register inputs
                player.RegisterInputSystem(callbackContext.control.device,ActionAsset);
            }
            else {
                Debug.LogError("Could not find Player on object");
            }
        }

        private void HandleRemovePlayer(Guid playerId) {
            if (!Players.TryGetValue(playerId, out var player)) {
                Debug.Log("Player doesn't exist");
                return;
            }

            RemovePlayer.Invoke(player.GetId());
            player.UnregisterInputSystem();
            player.gameObject.SetActive(false);
            Players.Remove(playerId);
        }
    }
}