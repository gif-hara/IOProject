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

        public void GiveDamage(Actor targetActor, int damage)
        {
            if (!isLocal)
            {
                return;
            }
            targetActor.RpcToAll("TakeDamage", this.strixReplicator.networkInstanceId, damage);
        }

        public void GiveDamage(StageChunk stageChunk, int damage)
        {
            if (!isLocal)
            {
                return;
            }
            RpcToAll("RpcGiveDamageStageChunk", this.strixReplicator.networkInstanceId, damage);
        }

        [StrixRpc]
        public void TakeDamage(long attackerNetworkInstanceId, int damage)
        {
            Debug.Log($"TakeDamage: attackerNetworkInstanceId = {attackerNetworkInstanceId}, myNetworkInstanceId = {this.strixReplicator.networkInstanceId}, damage = {damage}", this);
        }

        [StrixRpc]
        public void RpcGiveDamageStageChunk(int positionX, int positionY, int damage)
        {
            var stageController = TinyServiceLocator.Resolve<StageController>();
            var positionId = new Vector2Int(positionX, positionY);
        }
    }
}
