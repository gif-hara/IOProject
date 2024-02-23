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
    [AddTypeMenu("IOProject/GiveDamageActor")]
    [Serializable]
    public sealed class GiveDamageActor : ISequence
    {
        [SerializeField]
        private string ownerActorName;

        [SerializeField]
        private string targetActorName;

        [SerializeField]
        private int damage;

        public UniTask PlayAsync(Container container, CancellationToken cancellationToken)
        {
            var ownerActor = container.Resolve<Actor>(ownerActorName);
            var targetActor = container.Resolve<Actor>(targetActorName);
            ownerActor.GiveDamage(targetActor, damage);

            return UniTask.CompletedTask;
        }
    }
}
