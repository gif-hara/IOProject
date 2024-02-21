using IOProject.ActorControllers;
using UnityEngine;

namespace IOProject
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class StageChunk : MonoBehaviour
    {
        private StageChunkModel model;

        public void Setup(StageChunkModel model)
        {
            this.model = model;
        }

        public void TakeDamage(Actor actor, int damage)
        {
            if (!actor.isLocal)
            {
                return;
            }

            this.model.AddDamageMap(actor.strixReplicator.networkInstanceId, damage);
            Debug.Log($"TakeDamage: actor = {actor}, damage = {damage}", this);
        }
    }
}
