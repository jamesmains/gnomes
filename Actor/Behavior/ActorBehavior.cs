using System;
using parent_house_framework.Managed;
using Sirenix.OdinInspector;
using UnityEngine;

namespace gnomes.Actor.Behavior {
    [Serializable]
    public abstract class ActorBehavior: ManagedObject {
        [FoldoutGroup("Dependencies"), ReadOnly]
        public Gnome ParentGnome;
        
        public virtual void Update(){}
        public virtual void FixedUpdate(){}
        public virtual void HandleUse(){}
        public virtual void HandleInteract(){}
        public virtual void HandleKill(){}
        public virtual void HandleRevive(){}
        public virtual void HandleJump(){}
        public virtual void HandleMove(Vector2 moveVector){}
        public virtual void HandleAim(Vector2 aimVector){}
        public virtual void HandleChangeDetails(ActorDetails actorDetails){}
        

        #if UNITY_EDITOR
        [FoldoutGroup("Debug"), ShowIf("HasErrorMessage"), SerializeField, ReadOnly]
        protected string BehaviorErrorMessage = String.Empty;
        private bool HasErrorMessage ()=> BehaviorErrorMessage != String.Empty;
        
        [FoldoutGroup("Debug"), ShowIf("HasWarningMessage"), SerializeField, ReadOnly]
        protected string BehaviorWarningMessage = String.Empty;
        private bool HasWarningMessage ()=> BehaviorWarningMessage != String.Empty;
        #endif
        
        public virtual void Init(Gnome parentGnome) {
            ParentGnome = parentGnome;
            parentGnome.OnMove += HandleMove;
            parentGnome.OnJump += HandleJump;
            parentGnome.OnAim += HandleAim;
            parentGnome.OnUse += HandleUse;
            parentGnome.OnInteract += HandleInteract;
            parentGnome.OnKill += HandleKill;
            parentGnome.OnRevive += HandleRevive;
            parentGnome.OnChangeDetails += HandleChangeDetails;
        }
    }
}
