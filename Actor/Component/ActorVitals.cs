using gnomes.Actor.Behavior;
using gnomes.Actor.Behavior.Despawn;
using parent_house_framework.Values;
using Sirenix.OdinInspector;
using UnityEngine;

namespace gnomes.Actor.Component {
    public class ActorVitals: ActorComponent {
        [SerializeField, FoldoutGroup("Settings")]
        private ActorVitalsBehavior _vitalsBehavior;
        
        protected override void OnEnable() {
            base.OnEnable();
            FetchDependencies();
        }
    
        private void FetchDependencies() {
            _vitalsBehavior = _vitalsBehavior.Create<ActorVitalsBehavior>(this.Actor) as ActorVitalsBehavior;
        }

        private void Update() {
            _vitalsBehavior?.Update();
        }

        private void FixedUpdate() {
            _vitalsBehavior?.FixedUpdate();
        }
        
        protected override void HandleActorChanged(ActorDetails newDetails) {
        }
    
        protected override void HandleActorLifeStateChange(bool isAlive) {
            _vitalsBehavior.HandleActorLifeStateChange(isAlive);
        }
    }
}