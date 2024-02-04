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
                    actor.Model.Rotation *= Quaternion.Euler(0, look.x, 0);
                })
                .AddTo(actor.destroyCancellationToken);
        }
    }
}
