using System;
using System.Collections.Generic;
using R3;
using UnityEngine;

namespace IOProject
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public sealed class StageChunkModel
    {
        public Vector2Int PositionId { get; }

        private Dictionary<long, ReactiveProperty<int>> damageMap = new();

        public IReadOnlyDictionary<long, ReactiveProperty<int>> DamageMap => damageMap;

        private Subject<long> onAddDamageMapSubject = new();

        public Observable<long> OnAddDamageMap => onAddDamageMapSubject;

        private ReactiveProperty<long> occupiedNetworkId = new(-1);

        public ReadOnlyReactiveProperty<long> OccupiedNetworkId => occupiedNetworkId;

        private StrixMessageStageChunkModel strixMessage;

        public StrixMessageStageChunkModel StrixMessage => strixMessage;

        /// <summary>
        /// 占有されているか
        /// </summary>
        public bool IsOccupied => occupiedNetworkId.Value != -1;

        public StageChunkModel(Vector2Int positionId, StrixMessageStageChunkModel strixMessage)
        {
            this.PositionId = positionId;
            this.strixMessage = strixMessage;
            this.strixMessage.positionId = positionId;
        }

        public StageChunkModel(StrixMessageStageChunkModel message)
        {
            this.PositionId = message.positionId;
            this.strixMessage = message;
            foreach (var (networkInstanceId, damage) in message.damageMap)
            {
                damageMap.Add(networkInstanceId, new ReactiveProperty<int>(damage));
            }
            occupiedNetworkId = new ReactiveProperty<long>(message.occupiedNetworkId);
        }

        public void AddDamageMap(long networkInstanceId, int damage)
        {
            var damageReactiveProperty = GetOrCreateDamageReactiveProperty(networkInstanceId);
            damageReactiveProperty.Value += damage;
            strixMessage.SyncDamageMap(networkInstanceId, damageReactiveProperty.Value);
            TryOccupy(networkInstanceId);
        }

        public void SyncDamageMap(long networkInstanceId, int damage)
        {
            GetOrCreateDamageReactiveProperty(networkInstanceId).Value = damage;
            strixMessage.SyncDamageMap(networkInstanceId, damage);
            TryOccupy(networkInstanceId);
        }

        private ReactiveProperty<int> GetOrCreateDamageReactiveProperty(long networkInstanceId)
        {
            if (damageMap.TryGetValue(networkInstanceId, out var damageReactiveProperty))
            {
                return damageReactiveProperty;
            }
            damageReactiveProperty = new ReactiveProperty<int>(0);
            damageMap.Add(networkInstanceId, damageReactiveProperty);
            onAddDamageMapSubject.OnNext(networkInstanceId);
            return damageReactiveProperty;
        }

        private void TryOccupy(long networkInstanceId)
        {
            var gameDesignData = TinyServiceLocator.Resolve<GameDesignData>();
            var damageThreshold = IsOccupied ? gameDesignData.StageChunkOccupiedDamageThreshold : gameDesignData.StageChunkDefaultDamageThreshold;
            if (damageMap[networkInstanceId].Value >= damageThreshold)
            {
                foreach (var (key, value) in damageMap)
                {
                    value.Value = 0;
                    strixMessage.SyncDamageMap(key, 0);
                }
                occupiedNetworkId.Value = networkInstanceId;
                strixMessage.SyncOccupiedNetworkId(networkInstanceId);
            }
        }

        public void Sync(StrixMessageStageChunkModel message)
        {
            foreach (var (networkInstanceId, damage) in message.damageMap)
            {
                GetOrCreateDamageReactiveProperty(networkInstanceId).Value = damage;
                this.strixMessage.SyncDamageMap(networkInstanceId, damage);
            }
            occupiedNetworkId.Value = message.occupiedNetworkId;
            this.strixMessage.SyncOccupiedNetworkId(message.occupiedNetworkId);
        }
    }
}
