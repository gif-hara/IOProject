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

        public void Initialize(ActorModel model)
        {
            this.Model = model;
            this.LocatorController = new ActorLocatorController(locators);
        }
    }
}
