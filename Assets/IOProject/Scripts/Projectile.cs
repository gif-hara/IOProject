using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using IOProject.ActorControllers;
using UnityEngine;
using UnityEngine.Serialization;
using UnitySequencerSystem;

namespace IOProject
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Projectile : MonoBehaviour
    {
        [SerializeReference, SubclassSelector]
        private List<ISequence> onSpawnSequence;

        [SerializeReference, SubclassSelector]
        private List<ISequence> onHitActorSequence;

        private Actor owner;

        public void Spawn(Actor owner, Vector3 position, Quaternion rotation)
        {
            var projectile = Instantiate(this, position, rotation);
            projectile.owner = owner;
            projectile.InvokeSpawnSequence();
        }

        private void InvokeSpawnSequence()
        {
            var container = new Container();
            var sequencer = new Sequencer(container, onSpawnSequence);
            sequencer.PlayAsync(destroyCancellationToken).Forget();
        }

        void OnTriggerEnter(Collider other)
        {
            var targetActor = other.GetComponentInParent<Actor>();
            if (targetActor == null)
            {
                return;
            }
            if (owner == targetActor)
            {
                return;
            }
            var container = new Container();
            container.Register("OwnerActor", owner);
            container.Register("TargetActor", targetActor);
            var sequencer = new Sequencer(container, onHitActorSequence);
            sequencer.PlayAsync(destroyCancellationToken).Forget();
        }
    }
}
