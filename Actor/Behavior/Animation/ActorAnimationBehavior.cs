using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace gnomes.Actor.Behavior.Animation {
    [Serializable]
    public abstract class ActorAnimationBehavior : ActorBehavior
    {
        public override void Init(Gnome parentGnome) {
            base.Init(parentGnome);
            ParentGnome.OnSetAnimationFloat += HandleSetAnimationFloat;
            ParentGnome.OnSetAnimationBool += HandleSetAnimationBool;
        }

        public virtual void HandleSetAnimationBool(string s, bool b){}
        public virtual void HandleSetAnimationFloat(string s, float f){}
    }

    /// <summary>
    /// Basic Animation System
    /// Todo: Find more suitable name
    /// </summary>
    [Serializable]
    public class ActorAnimation : ActorAnimationBehavior {

        #region Properties
        
        [SerializeField, FoldoutGroup("Dependencies"), ReadOnly]
        private Animator Anim;
        
        #endregion
        
        #region Factory Methods and Constructors

        public override void Init(Gnome parentGnome) {
            base.Init(parentGnome);
            var animators = ParentGnome.GetComponentsInChildren<Animator>();
            BehaviorWarningMessage = animators.Length switch {
                > 1 => "Multiple animators found. This behavior relies on a single animator.",
                0 => "No animators found. This behavior relies on a single animator.",
                _ => String.Empty
            };
            Anim = animators[0];
        }

        #endregion
        
        #region Methods

        public override void HandleSetAnimationBool(string s, bool b) {
            Anim.SetBool(s,b);
        }

        public override void HandleSetAnimationFloat(string s, float f) {
            Debug.Log($"Name: {s}, Value: {f}");
            Anim.SetFloat(s,f);
        }

        #endregion
        
    }
}