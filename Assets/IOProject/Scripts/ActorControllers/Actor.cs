using System.Collections.Generic;
using SoftGear.Strix.Unity.Runtime;
using UnityEngine;

namespace IOProject.ActorControllers
{
    public class Actor : StrixBehaviour
    {
        [SerializeField]
        private List<ActorLocatorController.LocatorData> locators;

        public ActorLocatorController LocatorController { get; private set; }

        public IActorPostureController PostureController { get; private set; }

        public Actor Spawn()
        {
            var actor = Instantiate(this);
            actor.Initialize();
            ActorEvents.OnSpawned.Publish(actor);
            return actor;
        }

        private void Initialize()
        {
            this.LocatorController = new ActorLocatorController(locators);
            this.PostureController = GetComponent<IActorPostureController>();
            this.PostureController.Setup(this);
        }

        public void GiveDamage(Actor targetActor)
        {
            if (!isLocal)
            {
                return;
            }
            targetActor.RpcToAll("TakeDamage", 10);
        }

        [StrixRpc]
        public void TakeDamage(int damage)
        {
            Debug.Log($"TakeDamage: {damage}", this);
        }
    }
}
