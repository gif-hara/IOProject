using System.Collections.Generic;
using R3;
using SoftGear.Strix.Unity.Runtime;

namespace IOProject
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class StageChunkModel
    {
        private Dictionary<long, ReactiveProperty<int>> damageMap = new();

        public IReadOnlyDictionary<long, ReactiveProperty<int>> DamageMap => damageMap;

        private Subject<long> onAddDamageMapSubject = new();

        public Observable<long> OnAddDamageMap => onAddDamageMapSubject;

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
                onAddDamageMapSubject.OnNext(networkInstanceId);
            }
        }
    }
}
