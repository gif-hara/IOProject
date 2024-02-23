using System.Collections.Generic;
using System.Diagnostics;
using R3;
using UnityEngine;

namespace IOProject
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class StageChunkModel
    {
        public Vector2Int PositionId { get; }

        private Dictionary<long, ReactiveProperty<int>> damageMap = new();

        public IReadOnlyDictionary<long, ReactiveProperty<int>> DamageMap => damageMap;

        private Subject<long> onAddDamageMapSubject = new();

        public Observable<long> OnAddDamageMap => onAddDamageMapSubject;

        public StageChunkModel(Vector2Int positionId)
        {
            this.PositionId = positionId;
        }

        public void AddDamageMap(long networkInstanceId, int damage)
        {
            if (damageMap.TryGetValue(networkInstanceId, out var damageReactiveProperty))
            {
                damageReactiveProperty.Value += damage;
            }
            else
            {
                damageReactiveProperty = new ReactiveProperty<int>(damage);
                damageMap.Add(networkInstanceId, damageReactiveProperty);
            }
            onAddDamageMapSubject.OnNext(networkInstanceId);
            UnityEngine.Debug.Log($"AddDamageMap: networkInstanceId = {networkInstanceId}, damage = {damage}");
        }
    }
}
