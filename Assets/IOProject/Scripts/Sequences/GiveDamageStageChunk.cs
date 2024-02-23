using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using IOProject.ActorControllers;
using UnityEngine;
using UnitySequencerSystem;

namespace IOProject.Sequences
{
    /// <summary>
    /// <see cref="StageChunk"/>にダメージを与えるシーケンス
    /// </summary>
    [AddTypeMenu("IOProject/GiveDamageStageChunk")]
    [Serializable]
    public sealed class GiveDamageStageChunk : ISequence
    {
        [SerializeField]
        private string ownerActorName = "OwnerActor";

        [SerializeField]
        private string targetStageChunkName = "TargetStageChunk";

        [SerializeField]
        private int damage;

        public UniTask PlayAsync(Container container, CancellationToken cancellationToken)
        {
            var ownerActor = container.Resolve<Actor>(ownerActorName);
            var targetStageChunk = container.Resolve<StageChunk>(targetStageChunkName);
            ownerActor.GiveDamage(targetStageChunk, damage);

            return UniTask.CompletedTask;
        }
    }
}
