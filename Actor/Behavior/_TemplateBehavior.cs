namespace gnomes.Actor.Behavior {
    public class _TemplateBehavior : ActorBehavior {
        
        #region Factory Methods and Constructors

        /// <summary>
        /// Used to create and attach this component to an actor.
        /// Any custom settings that need to be carried over (i.e. MoveSpeed) should be set here.
        /// </summary>
        /// <param name="owner">Owner actor</param>
        /// <typeparam name="T">Type of behavior</typeparam>
        /// <returns></returns>
        public override ActorBehavior Create<T>(Actor owner) {
            return new _TemplateBehavior(owner);
        }

        /// <summary>
        /// Assign any external components here (i.e. TryGetComponent(out var rb))
        /// </summary>
        /// <param name="ownerActor"></param>
        public _TemplateBehavior(Actor ownerActor) : base(ownerActor) { }

        /// <summary>
        /// Empty constructor to allow for Odin to display this in the Inspector
        /// </summary>
        public _TemplateBehavior() { }

        #endregion

        #region Methods

        // Handled by parent component
        public override void Update() {
            throw new System.NotImplementedException();
        }

        // Handled by parent component
        public override void FixedUpdate() {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}