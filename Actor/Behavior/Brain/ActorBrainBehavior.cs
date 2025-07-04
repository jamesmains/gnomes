using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace gnomes.Actor.Behavior.Brain {
    [Serializable]
    public abstract class ActorBrain : ActorBehavior {

        [SerializeField, FoldoutGroup("Settings")]
        public float LeashDistance = 5f;

        [SerializeField, FoldoutGroup("Settings")]
        public float StopDistance = 1.5f;
    }

    [Serializable]
    public class MinionBrain : ActorBrain {

        // Todo: add seek function for finding leader?

        [SerializeField, FoldoutGroup("Debug"), ReadOnly]
        public Gnome LeaderActor;

        [SerializeField, FoldoutGroup("Debug"), ReadOnly]
        private float LastTimeSignaled;

        public override void Update() {
            Debug.Log("Testing minion brain!");
            return;
            if (!(Time.time > LastTimeSignaled + 1f)) return;
            LastTimeSignaled = Time.time;
            Vector3 RandomNewPosition = Vector3.zero;
            RandomNewPosition.x = Random.Range(-1f, 1f);
            RandomNewPosition.z = Random.Range(-1f, 1f);
            if (LeaderActor == null)
                ParentGnome.OnMove?.Invoke(RandomNewPosition);
            else {
                if (Vector3.Distance(ParentGnome.transform.position, LeaderActor.transform.position) < 1.5f) {
                    var moveAwayDirection = (ParentGnome.transform.position - LeaderActor.transform.position).normalized;
                    ParentGnome.OnMove?.Invoke(moveAwayDirection);
                }
                else if (Vector3.Distance(ParentGnome.transform.position, LeaderActor.transform.position) > 2f) {
                    ParentGnome.OnMove?.Invoke(LeaderActor.transform.position);
                }
            }
        }

        public override void FixedUpdate() {
        }
    }

    [Serializable]
    public class LeaderBrain : ActorBrain {
        public override void Update() {
        }

        public override void FixedUpdate() {
        }
    }
}