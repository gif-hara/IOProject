using R3;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

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
                    var position = x.message.position;
                    var positionId = actor.PostureController.PositionIdReactiveProperty.CurrentValue;
                    var diffId = positionId - localActor.PostureController.PositionIdReactiveProperty.CurrentValue;
                    actor.PostureController.Warp(new Vector3(
                        position.x + diffId.x * gameDesignData.StageChunkSize,
                        position.y,
                        position.z + diffId.y * gameDesignData.StageChunkSize
                        ));
                })
                .RegisterTo(actor.destroyCancellationToken);
            gameNetworkController
                .RoomRelayAsObservable()
                .WhereOwner(actor.NetworkController)
                .MatchMessage<NetworkMessage.UpdateActorPositionId>()
                .Subscribe(x =>
                {
                    actor.PostureController.SyncPositionId(x.message.positionId);
                })
                .RegisterTo(actor.destroyCancellationToken);
        }
    }
}
