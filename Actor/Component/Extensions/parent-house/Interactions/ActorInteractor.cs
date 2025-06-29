using System.Collections.Generic;
using parent_house_framework.Interactions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace gnomes.Actor.Component.Extensions.parent_house.Interactions {
    public enum InteractionDetectionMode {
        _2d,
        _3d
    }

    public class ActorInteractor : ActorComponent {
        [SerializeField, FoldoutGroup("Settings")]
        private InteractionDetectionMode DetectionMode = InteractionDetectionMode._3d;

        private readonly Dictionary<GameObject, IInteractable> NearbyTriggers = new();

        protected override void OnEnable() {
            base.OnEnable();
            Actor.OnInteract += TryInteract;
        }

        protected override void OnDisable() {
            base.OnDisable();
            Actor.OnInteract -= TryInteract;
        }

        private void TryInteract() {
            foreach (var trigger in NearbyTriggers) {
                trigger.Value.ChangeState();
            }
        }

        private void TryAddInteractable(GameObject targetObj) {
            if (!targetObj.TryGetComponent(out IInteractable pTrigger)) return;
            {
                if (pTrigger.RequireButtonToChangeState())
                    NearbyTriggers.TryAdd(targetObj, pTrigger);
                pTrigger.Notify(NotifyState.Entry);
            }
        }

        private void TryRemoveInteractable(GameObject targetObj) {
            if (!targetObj.TryGetComponent(out IInteractable pTrigger)) return;
            {
                pTrigger.Notify(NotifyState.Exit);
                if (NearbyTriggers.ContainsKey(targetObj))
                    NearbyTriggers.Remove(targetObj);
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (DetectionMode == InteractionDetectionMode._3d) return;
            TryAddInteractable(other.gameObject);
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (DetectionMode == InteractionDetectionMode._3d) return;
            TryRemoveInteractable(other.gameObject);
        }

        private void OnTriggerEnter(Collider other) {
            if (DetectionMode == InteractionDetectionMode._2d) return;
            TryAddInteractable(other.gameObject);
        }

        private void OnTriggerExit(Collider other) {
            if (DetectionMode == InteractionDetectionMode._2d) return;
            TryRemoveInteractable(other.gameObject);
        }
    }
}