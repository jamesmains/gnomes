using Sirenix.OdinInspector;
using UnityEngine;

namespace gnomes.Inventory {
    public class ItemObject: MonoBehaviour {
        [SerializeField, FoldoutGroup("Status"), ReadOnly]
        private ItemBehavior PickupBehavior;
        
        [SerializeField, FoldoutGroup("Status"), ReadOnly]
        private ItemBehavior UseBehavior;
        
        [SerializeField, FoldoutGroup("Status"), ReadOnly]
        private ItemBehavior DropBehavior;

        // public void HandlePickup(ActorHandsComponent targetComponent) {
        //     if(PickupBehavior.Invoke())
        //         Attach();
        // }

        public void Attach() {
            
        }
        
        public void HandleUse() {
            UseBehavior.Invoke();
        }

        public void HandleDrop() {
            if(DropBehavior.Invoke())
                Attach();
        }

        public void Detatch() {
            
        }
    }
}