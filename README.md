# GNOMES
### Good Nodes Over Messy Entity System
Not really the name. I forgot what GNOMES stood for. My bad.
## Actor System
### Actor
The root class for the actor system. This binds all the components and handles the event-driven system directed by an Actor Brain.
### Actor Brain
The logic for the actor that dictates their actions. Agnostic to what components the actor has. Simply attempts to drive forward actions.
### Components
If the brain is... well the brain... the components are the limbs. The brain thinks up actions, tells the actor, and the actor lets any components know what to try to do.
### Behaviors
User defined classes that dictate what it looks like when an actor performs an action.