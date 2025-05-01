using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gnomes.Actor.Behavior {
    [Serializable]
    public abstract class ActorBehavior {
        [FoldoutGroup("Dependencies"), ReadOnly]
        public Actor OwnerActor;
        
        public abstract ActorBehavior Create<T>(Actor owner);
        public abstract void Update();
        public abstract void FixedUpdate();

        #if UNITY_EDITOR
        [FoldoutGroup("Debug"), ShowIf("HasErrorMessage"), SerializeField, ReadOnly]
        protected string BehaviorErrorMessage = String.Empty;
        private bool HasErrorMessage ()=> BehaviorErrorMessage != String.Empty;
        
        [FoldoutGroup("Debug"), ShowIf("HasWarningMessage"), SerializeField, ReadOnly]
        protected string BehaviorWarningMessage = String.Empty;
        private bool HasWarningMessage ()=> BehaviorWarningMessage != String.Empty;
        #endif
        
        protected ActorBehavior(Actor ownerActor) {
            OwnerActor = ownerActor;
        }
        
        protected ActorBehavior() {}
    }
}
