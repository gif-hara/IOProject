using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;

namespace IOProject.ActorControllers
{
    public class PlayerActorController
    {
        public void Attach(Actor actor, PlayerSpec playerSpec)
        {
            var inputController = TinyServiceLocator.Resolve<IInputController>();
            var fireCoolTime = 0.0f;
            var canFire = false;
            var gameDesignData = TinyServiceLocator.Resolve<GameDesignData>();
            actor.LocatorController.GetLocator("View.ThirdPerson").gameObject.SetActive(false);
            actor.LocatorController.GetLocator("View.FirstPerson").gameObject.SetActive(true);
            actor.GetAsyncUpdateTrigger()
                .Subscribe(_ =>
                {
                    var look = inputController.Actions.InGame.Look.ReadValue<Vector2>() * playerSpec.rotateSpeed;
                    actor.PostureController.AddRotate(new Vector3(-look.y, look.x, 0));
                    var move = inputController.Actions.InGame.Move.ReadValue<Vector2>() * playerSpec.moveSpeed;
                    var yOnlyRotation = Quaternion.Euler(0, actor.transform.rotation.eulerAngles.y, 0);
                    var forward = yOnlyRotation * Vector3.forward;
                    var right = yOnlyRotation * Vector3.right;
                    var vector = forward * move.y + right * move.x;
                    actor.PostureController.AddMove(new Vector3(vector.x, 0.0f, vector.z) * Time.deltaTime);
                    fireCoolTime -= Time.deltaTime;
                    if (fireCoolTime < 0.0f && canFire)
                    {
                        fireCoolTime = playerSpec.fireCoolTime;
                        var firePoint = actor.LocatorController.GetLocator("FirePoint");
                        gameDesignData.ProjectilePrefab.Spawn(actor, firePoint.position, firePoint.rotation);
                    }
                })
                .AddTo(actor.destroyCancellationToken);
            inputController.Actions.InGame.Fire.OnPerformedAsync()
                .Subscribe(_ =>
                {
                    canFire = true;
                })
                .AddTo(actor.destroyCancellationToken);
            inputController.Actions.InGame.Fire.OnCanceledAsync()
                .Subscribe(_ =>
                {
                    canFire = false;
                })
                .AddTo(actor.destroyCancellationToken);
        }
    }
}
