using Cysharp.Threading.Tasks;
using R3;
using R3.Triggers;
using UnityEngine;

namespace IOProject.ActorControllers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ActorRemoteController
    {
        public void Begin(Actor actor)
        {
            var gameNetworkController = TinyServiceLocator.Resolve<GameNetworkController>();
            var gameDesignData = TinyServiceLocator.Resolve<GameDesignData>();
            var sendTiming = 1.0f / gameDesignData.ActorRemoteSendFrequency;
            var currentSendTime = 0.0f;

            actor.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    if (!actor.NetworkController.isLocal)
                    {
                        return;
                    }
                    currentSendTime += Time.deltaTime;
                    if (currentSendTime < sendTiming)
                    {
                        return;
                    }
                    currentSendTime = 0.0f;
                    gameNetworkController.SendRoomRelayAsync(new NetworkMessage.UpdateActorPosition
                    {
                        position = actor.transform.position,
                        positionId = actor.PostureController.PositionIdReactiveProperty.CurrentValue
                    })
                    .Forget();
                    gameNetworkController.SendRoomRelayAsync(new NetworkMessage.UpdateActorRotation
                    {
                        rotation = new Vector3(
                            actor.PostureController.RotationX,
                            actor.PostureController.RotationY,
                            0.0f
                            )
                    })
                    .Forget();
                })
                .RegisterTo(actor.destroyCancellationToken);

            gameNetworkController
                .RoomRelayAsObservable()
                .WhereOwner(actor.NetworkController)
                .MatchMessage<NetworkMessage.UpdateActorPosition>()
                .Subscribe(x =>
                {
                    var localActor = TinyServiceLocator.Resolve<Actor>("LocalActor");
                    if (localActor == null)
                    {
                        return;
                    }
                    actor.PostureController.SyncPositionId(x.message.positionId);
                    var position = x.message.position;
                    var positionId = actor.PostureController.PositionIdReactiveProperty.CurrentValue;
                    var diffId = positionId - localActor.PostureController.PositionIdReactiveProperty.CurrentValue;
                    actor.PostureController.SyncPosition(new Vector3(
                        position.x + diffId.x * gameDesignData.StageChunkSize,
                        position.y,
                        position.z + diffId.y * gameDesignData.StageChunkSize
                        ));
                })
                .RegisterTo(actor.destroyCancellationToken);
            gameNetworkController
                .RoomRelayAsObservable()
                .WhereOwner(actor.NetworkController)
                .MatchMessage<NetworkMessage.UpdateActorRotation>()
                .Subscribe(x =>
                {
                    actor.PostureController.SyncRotation(x.message.rotation);
                })
                .RegisterTo(actor.destroyCancellationToken);
        }
    }
}
