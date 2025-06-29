using System;
using System.Collections.Generic;
using gnomes.Actor.Behavior.Brain;
using gnomes.Spawner;
using parent_house_framework.Values;
using Sirenix.OdinInspector;
using UnityEngine;

namespace gnomes.Actor {
    public class Actor : Spawnable {
        [SerializeField, FoldoutGroup("Settings")] [Tooltip("Leave off for Actor Spawner")]
        private bool SetActorOnStart;

        [SerializeField, FoldoutGroup("Settings"), ShowIf(nameof(SetActorOnStart))]
        private ActorDetails DefaultActor;

        [SerializeField, FoldoutGroup("Status"), ReadOnly]
        private ActorDetails CurrentActorDetails;
        
        [SerializeField, FoldoutGroup("Status"), ReadOnly]
        public bool Possessed;

        [SerializeField, FoldoutGroup("Status"), ReadOnly]
        private float TimeTillDespawn;

        private ActorBrain Brain;
        private bool BrainIsActive() => Brain != null && !Possessed && IsActive();

        public bool IsActive() => BindingBool.TrueForAll(Bindings);
        public readonly Dictionary<Guid, BindingBool> Bindings = new();

        // Todo: need to decouple these from this class
        public float CurrentLeashDistance => Brain?.LeashDistance ?? 1;
        public float CurrentStopDistance => Brain?.StopDistance ?? 1;

        public ActorDetails Details {
            get => CurrentActorDetails;
            private set {
                CurrentActorDetails = value;
                HandleSwapActor();
            }
        }

        // Setup
        public Action<ActorDetails> OnActorSet;

        // Possession
        public Action<Guid> OnPossessed;
        public Action<Guid> OnReleasePossession;

        // Actions
        public Action<Vector3> OnMove;
        public Action<Vector2> OnAim;
        public Action OnUse;
        public Action OnInteract;

        // Vitals
        public Action<bool> OnLifeStateChange;

        private void Start() {
            Details = DefaultActor;
        }

        protected override void OnEnable() {
            base.OnEnable();
            OnPossessed += HandlePossession;
            OnReleasePossession += HandleReleasePossession;
            OnSpawn += delegate { OnLifeStateChange?.Invoke(true); };
        }

        protected override void OnDisable() {
            base.OnDisable();
            OnPossessed -= HandlePossession;
            OnReleasePossession -= HandleReleasePossession;
            OnSpawn -= delegate { OnLifeStateChange?.Invoke(true); };
            Despawn();
        }

        private void Update() {
            if (BrainIsActive())
                Brain.Update();
        }

        private void FixedUpdate() {
            if (BrainIsActive())
                Brain.Update();
        }

        private void HandlePossession(Guid ownerId) {
            Possessed = true;
        }

        [FoldoutGroup("Buttons"), Button]
        private void HandleReleasePossession(Guid ownerId) {
            Possessed = false;
        }

        [FoldoutGroup("Buttons"), Button]
        public void ReviveActor() {
            OnLifeStateChange?.Invoke(true);
        }

        [FoldoutGroup("Buttons"), Button]
        public void KillActor() {
            OnLifeStateChange?.Invoke(false);
        }

        [FoldoutGroup("Buttons"), Button]
        public void SwapActor(ActorDetails newDetails) {
            Details = newDetails;
        }

        private void HandleSwapActor() {
            OnActorSet.Invoke(Details);
            Brain = Details?.BrainBehavior?.Create<ActorBrain>(this) as ActorBrain;
        }
    }
}