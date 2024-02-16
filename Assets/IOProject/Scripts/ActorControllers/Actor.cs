using System.Collections.Generic;
using UnityEngine;

namespace IOProject.ActorControllers
{
    public class Actor : MonoBehaviour
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
    }
}
