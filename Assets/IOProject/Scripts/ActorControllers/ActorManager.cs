using System.Collections.Generic;
using SoftGear.Strix.Client.Core;

namespace IOProject.ActorControllers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ActorManager
    {
        private List<Actor> actors = new();

        public void AddActor(Actor actor)
        {
            actors.Add(actor);
        }

        public Actor GetActor(UID uid)
        {
            return actors.Find(x => x.NetworkController.strixReplicator.ownerUid.Equals(uid));
        }
    }
}
