using System;

namespace gnomes.Interfaces {
    public interface ISpawnable {
        public Action<ISpawnable> OnSpawn{ get; set; }
        public Action<ISpawnable> OnDespawn { get; set; }

        public void Spawn();
        public void Despawn();
    }
}