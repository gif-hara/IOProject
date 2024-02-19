using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using IOProject.ActorControllers;
using UnityEngine;
using UnitySequencerSystem;

namespace IOProject
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Projectile : MonoBehaviour
    {
        [SerializeReference, SubclassSelector]
        private List<ISequence> onEnableSequence;

        [SerializeReference, SubclassSelector]
        private List<ISequence> onTriggerEnterSequence;

        void OnEnable()
        {
            var container = new Container();
            var sequencer = new Sequencer(container, onEnableSequence);
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
