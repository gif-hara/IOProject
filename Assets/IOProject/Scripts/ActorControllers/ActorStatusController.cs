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
        private readonly Actor actor;

        private readonly ReactiveProperty<int> hitPointMax = new();

        private readonly ReactiveProperty<int> hitPoint = new();

        public ReadOnlyReactiveProperty<int> HitPointMaxReactiveProperty => hitPointMax;

        public ReadOnlyReactiveProperty<int> HitPointReactiveProperty => hitPoint;

        public ActorStatusController(Actor actor, int hitPointMax)
        {
            this.actor = actor;
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
            Debug.Log($"SyncHitPoint: networkInstanceId = {actor.NetworkController.strixReplicator.networkInstanceId}, HitPoint = {hitPoint}");
        }
    }
}
