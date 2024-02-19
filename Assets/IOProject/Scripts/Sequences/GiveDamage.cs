using System.Threading;
using Cysharp.Threading.Tasks;
using IOProject.ActorControllers;
using UnityEngine;
using UnitySequencerSystem;

namespace IOProject.Sequences
{
    /// <summary>
    /// <see cref="Actor"/>にダメージを与えるシーケンス
    /// </summary>
    public sealed class GiveDamage : ISequence
    {
        [SerializeField]
        private string targetActorName;

        public UniTask PlayAsync(Container container, CancellationToken cancellationToken)
        {
            var targetActor = container.Resolve<Actor>(targetActorName);
            Debug.Log($"GiveDamage: {targetActor}");

            return UniTask.CompletedTask;
        }
    }
}
