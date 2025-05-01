namespace Gnomes.Actor.Behavior {
    public class _TemplateBehavior : ActorBehavior {
        public override ActorBehavior Create<T>(Actor owner) {
            return new _TemplateBehavior(owner);
        }

        public _TemplateBehavior(Actor ownerActor) : base(ownerActor) {
        }

        public override void Update() {
            throw new System.NotImplementedException();
        }

        public override void FixedUpdate() {
            throw new System.NotImplementedException();
        }
    }
}