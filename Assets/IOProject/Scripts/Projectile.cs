using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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

        void OnEnable()
        {
            var container = new Container();
            var sequencer = new Sequencer(container, onEnableSequence);
            sequencer.PlayAsync(destroyCancellationToken).Forget();
        }
    }
}
