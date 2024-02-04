using System.Collections.Generic;
using UnityEngine;

namespace IOProject.ActorControllers
{
    public class Actor : MonoBehaviour
    {
        [SerializeField]
        private List<ActorLocatorController.LocatorData> locators;

        [SerializeField]
        private List<Transform> positionTransforms;

        [SerializeField]
        private List<Transform> rotationXTransforms;

        [SerializeField]
        private List<Transform> rotationYTransforms;

        public ActorModel Model { get; private set; }

        public ActorLocatorController LocatorController { get; private set; }

        public ActorPostureController PostureController { get; private set; }

        public void Initialize(ActorModel model)
        {
            this.Model = model;
            this.LocatorController = new ActorLocatorController(locators);
            this.PostureController = new ActorPostureController(
                this,
                positionTransforms,
                rotationXTransforms,
                rotationYTransforms
                );
        }
    }
}
