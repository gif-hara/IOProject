using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;

namespace IOProject.ActorControllers
{
    public class PlayerActorController
    {
        public void Attach(Actor actor)
        {
            actor.LocatorController.GetLocator("View.ThirdPerson").gameObject.SetActive(false);
            actor.LocatorController.GetLocator("View.FirstPerson").gameObject.SetActive(true);
            actor.GetAsyncUpdateTrigger()
                .Subscribe(_ =>
                {
                    var inputController = TinyServiceLocator.Resolve<IInputController>();
                    var look = inputController.Actions.InGame.Look.ReadValue<Vector2>();
                    actor.Model.Rotation *= Quaternion.Euler(look.y, look.x, 0);
                    var move = inputController.Actions.InGame.Move.ReadValue<Vector2>();
                    var yOnlyRotation = Quaternion.Euler(0, actor.Model.Rotation.eulerAngles.y, 0);
                    var forward = yOnlyRotation * Vector3.forward;
                    var right = yOnlyRotation * Vector3.right;
                    var vector = forward * move.y + right * move.x;
                    actor.Model.Position += new Vector2(vector.x, vector.z) * Time.deltaTime;
                })
                .AddTo(actor.destroyCancellationToken);
        }
    }
}
