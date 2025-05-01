using Sirenix.OdinInspector;
using UnityEngine;

namespace Gnomes.Actor.Component {
    public class ActorComponent: SerializedMonoBehaviour {
    
        [SerializeField, FoldoutGroup("Dependencies"),ReadOnly]
        public Actor Actor;

        protected virtual void OnEnable() {
            if(Actor == null) Actor = GetComponent<Actor>();
        
            Actor.OnMoveActor += HandleMove;
            Actor.OnPossessed += HandlePossession;
            Actor.OnReleasePossession += HandleReleasePossession;
            Actor.OnActorSet += HandleActorChanged;
        }
    
        protected virtual void OnDisable() {
            Actor.OnMoveActor -= HandleMove;
            Actor.OnPossessed -= HandlePossession;
            Actor.OnReleasePossession -= HandleReleasePossession;
            Actor.OnActorSet -= HandleActorChanged;
        }
    
        protected virtual void HandleActorChanged(ActorDetails newDetails) {
        
        }

        protected virtual void HandleActorDeath() {
        
        }

        protected virtual void HandlePossession(Actor actor) {
            
        }

        protected virtual void HandleReleasePossession(Actor actor) {
            
        }

        protected virtual void HandleMove(Vector3 targetVector, bool asDirection) {
            
        }
    }
}