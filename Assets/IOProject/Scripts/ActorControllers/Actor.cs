using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using R3;
using R3.Triggers;
using SoftGear.Strix.Unity.Runtime;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace IOProject.ActorControllers
{
    public class Actor : StrixBehaviour
    {
        [SerializeField]
        private List<ActorLocatorController.LocatorData> locators;

        public ActorLocatorController LocatorController { get; private set; }

        public IActorPostureController PostureController { get; private set; }

        void Awake()
        {
            this.LocatorController = new ActorLocatorController(locators);
            this.PostureController = GetComponent<IActorPostureController>();
            this.PostureController.Setup(this);
            var gameNetworkController = TinyServiceLocator.Resolve<GameNetworkController>();
            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    if (!isLocal)
                    {
                        return;
                    }
                    if (Keyboard.current.qKey.wasPressedThisFrame)
                    {
                        Debug.Log("qKey.wasPressedThisFrame", this);
                        gameNetworkController.SendRoomRelayAsync(new NetworkMessage.Helloworld { message = $"Hello, world! {this.strixReplicator.networkInstanceId}" }).Forget();
                    }
                })
                .RegisterTo(this.destroyCancellationToken);
            gameNetworkController.RoomRelayAsObservable()
                .Where(x => x.Data.GetFromUid().Equals(this.strixReplicator.ownerUid))
                .Subscribe(args =>
                {
                    Debug.Log($"RoomRelayAsObservable: {args}", this);
                })
                .RegisterTo(this.destroyCancellationToken);
        }

        public Actor Spawn()
        {
            var actor = Instantiate(this);
            ActorEvents.OnSpawned.Publish(actor);
            return actor;
        }

        public void GiveDamage(Actor targetActor, int damage)
        {
            if (!isLocal)
            {
                return;
            }
            targetActor.RpcToAll(nameof(TakeDamage), this.strixReplicator.networkInstanceId, damage);
        }

        public void GiveDamage(StageChunk stageChunk, int damage)
        {
            if (!isLocal)
            {
                return;
            }
            var positionId = stageChunk.Model.PositionId;
            RpcToRoomOwner(nameof(RpcGiveDamageStageChunk), positionId.x, positionId.y, damage);
        }

        public void SendPosition()
        {
            if (!isLocal)
            {
                return;
            }
            RpcToAll(
                nameof(SyncPosition),
                transform.position.x,
                transform.position.y,
                transform.position.z,
                PostureController.PositionIdReactiveProperty.CurrentValue.x,
                PostureController.PositionIdReactiveProperty.CurrentValue.y
            );
        }

        [StrixRpc]
        public void SyncPosition(float x, float y, float z, int positionIdX, int positionIdY)
        {
            if (isLocal)
            {
                return;
            }
            if (!TinyServiceLocator.Contains<Actor>("LocalActor"))
            {
                return;
            }
            var positionId = new Vector2Int(positionIdX, positionIdY);
            PostureController.SyncPositionId(positionId);
            var localPositionId = TinyServiceLocator.Resolve<Actor>("LocalActor").PostureController.PositionIdReactiveProperty.CurrentValue;
            var gameDesignData = TinyServiceLocator.Resolve<GameDesignData>();
            var diff = PostureController.PositionIdReactiveProperty.CurrentValue - localPositionId;
            transform.position = new Vector3(
                x + diff.x * gameDesignData.StageChunkSize,
                y,
                z + diff.y * gameDesignData.StageChunkSize
            );
            Debug.Log($"SyncPosition: positionId = {positionId}, localPositionId = {localPositionId}, diff = {diff}, transform.position = {transform.position}", this);
        }

        [StrixRpc]
        public void TakeDamage(long attackerNetworkInstanceId, int damage)
        {
            Debug.Log($"TakeDamage: attackerNetworkInstanceId = {attackerNetworkInstanceId}, myNetworkInstanceId = {this.strixReplicator.networkInstanceId}, damage = {damage}", this);
        }

        [StrixRpc]
        public void RpcGiveDamageStageChunk(int positionX, int positionY, int damage)
        {
            TinyServiceLocator.Resolve<Stage>().TakeDamageStageChunk(this.strixReplicator.networkInstanceId, new Vector2Int(positionX, positionY), damage);
        }
    }
}
