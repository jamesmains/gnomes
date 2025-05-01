namespace Gnomes.Actor {
    /// <summary>
    /// Unused ATM
    /// 
    /// </summary>
    public class ActorInformation {
        private readonly Actor Actor;
        // Contains information about the actor this is attached to.
        public ActorInformation(Actor actor) {
            Actor = actor;
            GetInformation();
        }

        public ActorInformation GetInformation() {
            return this;
        }
    }
}
