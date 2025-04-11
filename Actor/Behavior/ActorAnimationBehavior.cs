using System;
using Gnomes.Actor;
using Gnomes.Actor.Behavior;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public abstract class ActorAnimationBehavior : ActorBehavior
{
    public virtual void PlayAnimationByName(string animationName) {
        
    }
    
    protected ActorAnimationBehavior(Actor ownerActor) : base(ownerActor) {
    }
}

[Serializable]
public class ActorAnimation : ActorAnimationBehavior {
    
    public ActorAnimation(Actor ownerActor) : base(ownerActor) {
    }
    
    public override ActorBehavior Create<T>(Actor owner) {
        return new ActorAnimation(owner);
    }

    [SerializeField, FoldoutGroup("Debug"), ReadOnly]
    private Animator Anim;

    public override void PlayAnimationByName(string animationName) {
        base.PlayAnimationByName(animationName);
        Anim.SetTrigger(animationName);
    }

    public override void Update() {
    }

    public override void FixedUpdate() {
    }

    
}
