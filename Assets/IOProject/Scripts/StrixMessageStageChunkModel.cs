using System;
using System.Collections.ObjectModel;
using SerializableCollections;
using UnityEngine;

namespace IOProject
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public sealed class StrixMessageStageChunkModel
    {
        public Vector2Int positionId;

        public DamageDictionary damageMap = new();

        public long occupiedNetworkId = -1;

        public StrixMessageStageChunkModel()
        {
        }

        public StrixMessageStageChunkModel(StageChunkModel stageChunkModel)
        {
            this.positionId = stageChunkModel.PositionId;
            this.damageMap = new DamageDictionary();
            foreach (var (networkInstanceId, damage) in stageChunkModel.DamageMap)
            {
                this.damageMap.Add(networkInstanceId, damage.Value);
            }
            this.occupiedNetworkId = stageChunkModel.OccupiedNetworkId.CurrentValue;
        }

        public void SyncDamageMap(long networkInstanceId, int damage)
        {
            if (damageMap.ContainsKey(networkInstanceId))
            {
                damageMap[networkInstanceId] = damage;
            }
            else
            {
                damageMap.Add(networkInstanceId, damage);
            }
        }

        public void SyncOccupiedNetworkId(long networkInstanceId)
        {
            occupiedNetworkId = networkInstanceId;
        }
    }

    public sealed class StrixMessageStageChunkModelDictionary : SerializableDictionary<Vector2Int, StrixMessageStageChunkModel>
    {
    }

    public sealed class DamageDictionary : SerializableDictionary<long, int>
    {
    }
}
