using System.Collections.Generic;
using UnityEngine;

namespace IOProject.ActorControllers
{
    public class Actor : MonoBehaviour
    {
        [SerializeField]
        private List<ActorLocatorController.LocatorData> locators;

        public ActorModel Model { get; private set; }

        public ActorLocatorController LocatorController { get; private set; }

        public IActorPostureController PostureController { get; private set; }

        public Actor Spawn(ActorModel model)
        {
            var actor = Instantiate(this);
            actor.Initialize(model);
            ActorEvents.OnSpawned.Publish(actor);
            return actor;
        }

        private void Initialize(ActorModel model)
        {
            this.Model = model;
            this.LocatorController = new ActorLocatorController(locators);
            this.PostureController = GetComponent<IActorPostureController>();
            this.PostureController.Setup(this);
        }
    }
}
