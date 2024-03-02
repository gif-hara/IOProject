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
        private bool canFire = false;

        private float currentFireCoolTime = 0.0f;

        public void Setup(Actor actor)
        {
            actor.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    var gameDesignData = TinyServiceLocator.Resolve<GameDesignData>();
                    currentFireCoolTime -= Time.deltaTime;
                    var fireCoolTime = gameDesignData.FireCoolTime;
                    if (currentFireCoolTime < 0.0f && canFire)
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
            canFire = true;
        }

        public void EndFire()
        {
            canFire = false;
        }
    }
}
