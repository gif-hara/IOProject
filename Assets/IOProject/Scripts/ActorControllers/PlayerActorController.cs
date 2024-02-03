using Cysharp.Threading.Tasks.Linq;
using Cysharp.Threading.Tasks.Triggers;

namespace IOProject.ActorControllers
{
    public class PlayerActorController
    {
        public void Attach(Actor actor)
        {
            actor.GetAsyncUpdateTrigger()
                .Subscribe(_ =>
                {
                });
        }
    }
}
