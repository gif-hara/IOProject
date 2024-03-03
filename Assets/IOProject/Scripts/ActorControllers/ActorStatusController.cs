using IOProject.ActorControllers;
using R3;
using UnityEngine;

namespace IOProject.ActorControllers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ActorStatusController
    {
        private readonly ReactiveProperty<int> hitPointMax = new();

        private readonly ReactiveProperty<int> hitPoint = new();

        public ReadOnlyReactiveProperty<int> HitPointMaxReactiveProperty => hitPointMax;

        public ReadOnlyReactiveProperty<int> HitPointReactiveProperty => hitPoint;

        public ActorStatusController(int hitPointMax)
        {
            this.hitPointMax.Value = hitPointMax;
            this.hitPoint.Value = hitPointMax;
        }

        public void TakeDamage(int damage)
        {
            hitPoint.Value -= damage;
        }

        public void SyncHitPointMax(int hitPointMax)
        {
            this.hitPointMax.Value = hitPointMax;
        }

        public void SyncHitPoint(int hitPoint)
        {
            this.hitPoint.Value = hitPoint;
        }
    }
}
