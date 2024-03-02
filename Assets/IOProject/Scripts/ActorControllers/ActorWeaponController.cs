using IOProject.ActorControllers;
using R3;
using R3.Triggers;
using UnityEngine;

namespace IOProject
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ActorWeaponController
    {
        private readonly ReactiveProperty<bool> canFire = new(false);

        private float currentFireCoolTime = 0.0f;

        public ReadOnlyReactiveProperty<bool> CanFireReactiveProperty => canFire;

        public void Setup(Actor actor)
        {
            actor.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    var gameDesignData = TinyServiceLocator.Resolve<GameDesignData>();
                    currentFireCoolTime -= Time.deltaTime;
                    var fireCoolTime = gameDesignData.FireCoolTime;
                    if (currentFireCoolTime < 0.0f && canFire.Value)
                    {
                        currentFireCoolTime = fireCoolTime;
                        var firePoint = actor.LocatorController.GetLocator("FirePoint");
                        gameDesignData.ProjectilePrefab.Spawn(actor, firePoint.position, firePoint.rotation);
                    }
                })
                .RegisterTo(actor.destroyCancellationToken);
        }

        public void BeginFire()
        {
            canFire.Value = true;
        }

        public void EndFire()
        {
            canFire.Value = false;
        }

        public void SyncCanFire(bool canFire)
        {
            this.canFire.Value = canFire;
        }
    }
}
