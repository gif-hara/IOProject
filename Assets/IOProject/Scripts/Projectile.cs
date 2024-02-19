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
        [SerializeReference, SubclassSelector, FormerlySerializedAs("onEnableSequence")]
        private List<ISequence> onSpawnSequence;

        [SerializeReference, SubclassSelector]
        private List<ISequence> onTriggerEnterSequence;

        public void Spawn(Vector3 position, Quaternion rotation)
        {
            var projectile = Instantiate(this, position, rotation);
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
            var container = new Container();
            container.Register("TargetActor", other.GetComponentInParent<Actor>());
            var sequencer = new Sequencer(container, onTriggerEnterSequence);
            sequencer.PlayAsync(destroyCancellationToken).Forget();
        }
    }
}
