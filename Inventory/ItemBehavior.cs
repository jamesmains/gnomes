namespace gnomes.Inventory {
    public class ItemBehavior {
        public ItemBehavior(ItemObject ownerItem) {
            OwnerItem = ownerItem;
        }

        protected ItemObject OwnerItem;
        public virtual bool Invoke() {
            return true;
        }
    }

    public class ItemDropBehavior : ItemBehavior {
        public ItemDropBehavior(ItemObject ownerItem) : base(ownerItem) { }
        
    }
}