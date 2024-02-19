using System;
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
    [AddTypeMenu("IOProject/GiveDamage")]
    [Serializable]
    public sealed class GiveDamage : ISequence
    {
        [SerializeField]
        private string ownerActorName;

        [SerializeField]
        private string targetActorName;

        public UniTask PlayAsync(Container container, CancellationToken cancellationToken)
        {
            var ownerActor = container.Resolve<Actor>(ownerActorName);
            var targetActor = container.Resolve<Actor>(targetActorName);
            ownerActor.GiveDamage(targetActor);

            return UniTask.CompletedTask;
        }
    }
}
