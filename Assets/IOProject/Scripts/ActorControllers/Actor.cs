using System.Collections.Generic;
using UnityEngine;

namespace IOProject.ActorControllers
{
    public class Actor : MonoBehaviour
    {
        [SerializeField]
        private List<ActorLocatorController.LocatorData> locators;

        [SerializeField]
        private ActorNetworkController networkController;

        public ActorLocatorController LocatorController { get; private set; }

        public IActorPostureController PostureController { get; private set; }

        public ActorNetworkController NetworkController => this.networkController;

        public ActorWeaponController WeaponController { get; private set; }

        void Awake()
        {
            this.LocatorController = new ActorLocatorController(locators);
            this.PostureController = GetComponent<IActorPostureController>();
            this.PostureController.Setup(this);
            this.WeaponController = new ActorWeaponController();
            this.WeaponController.Setup(this);
        }

        public Actor Spawn()
        {
            var actor = Instantiate(this);
            ActorEvents.OnSpawned.Publish(actor);
            return actor;
        }
    }
}
