using Cysharp.Threading.Tasks;
using R3;
using R3.Triggers;
using SoftGear.Strix.Client.Core;
using SoftGear.Strix.Unity.Runtime;
using UnityEngine;

namespace IOProject.ActorControllers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ActorNetworkController : StrixBehaviour
    {
        [SerializeField]
        private Actor actor;

        async void Start()
        {
            await UniTask.WaitUntil(() => this.strixReplicator.didStart);

            var gameNetworkController = TinyServiceLocator.Resolve<GameNetworkController>();
            var gameDesignData = TinyServiceLocator.Resolve<GameDesignData>();
            var sendTiming = 1.0f / gameDesignData.ActorRemoteSendFrequency;
            var currentSendTime = 0.0f;

            actor.UpdateAsObservable()
                .Where(_ => actor.NetworkController.isLocal)
                .Subscribe(_ =>
                {
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

            actor.WeaponController.CanFireReactiveProperty
                .Where(_ => actor.NetworkController.isLocal)
                .Subscribe(x =>
                {
                    gameNetworkController.SendRoomRelayAsync(new NetworkMessage.UpdateCanFire
                    {
                        canFire = x
                    })
                    .Forget();
                })
                .RegisterTo(actor.destroyCancellationToken);

            actor.StatusController.HitPointMaxReactiveProperty
                .Where(_ => actor.NetworkController.isLocal)
                .Subscribe(x =>
                {
                    gameNetworkController.SendRoomRelayAsync(new NetworkMessage.UpdateHitPointMax
                    {
                        hitPointMax = x
                    })
                    .Forget();
                })
                .RegisterTo(actor.destroyCancellationToken);

            actor.StatusController.HitPointReactiveProperty
                .Where(_ => actor.NetworkController.isLocal)
                .Subscribe(x =>
                {
                    gameNetworkController.SendRoomRelayAsync(new NetworkMessage.UpdateHitPoint
                    {
                        hitPoint = x
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
                    var localActor = TinyServiceLocator.TryResolve<Actor>("LocalActor");
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
            gameNetworkController
                .RoomRelayAsObservable()
                .WhereOwner(actor.NetworkController)
                .MatchMessage<NetworkMessage.UpdateCanFire>()
                .Subscribe(x =>
                {
                    actor.WeaponController.SyncCanFire(x.message.canFire);
                })
                .RegisterTo(actor.destroyCancellationToken);
            gameNetworkController
                .RoomRelayAsObservable()
                .WhereOwner(actor.NetworkController)
                .MatchMessage<NetworkMessage.UpdateHitPointMax>()
                .Subscribe(x =>
                {
                    actor.StatusController.SyncHitPointMax(x.message.hitPointMax);
                })
                .RegisterTo(actor.destroyCancellationToken);
            gameNetworkController
                .RoomRelayAsObservable()
                .WhereOwner(actor.NetworkController)
                .MatchMessage<NetworkMessage.UpdateHitPoint>()
                .Subscribe(x =>
                {
                    actor.StatusController.SyncHitPoint(x.message.hitPoint);
                })
                .RegisterTo(actor.destroyCancellationToken);
            gameNetworkController
                .RoomRelayAsObservable()
                .WhereOwner(actor.NetworkController)
                .MatchMessage<NetworkMessage.GiveDamageActor>()
                .Subscribe(x =>
                {
                    var target = TinyServiceLocator.Resolve<ActorManager>().GetActor(x.message.target);
                    target.StatusController.TakeDamage(x.message.damage);
                })
                .RegisterTo(actor.destroyCancellationToken);
        }

        public UniTask SendRoomRelayAsync<T>(T message)
        {
            if (!isLocal)
            {
                Debug.LogError("SendRoomRelayAsync: !isLocal", this);
                return UniTask.CompletedTask;
            }
            return TinyServiceLocator.Resolve<GameNetworkController>().SendRoomRelayAsync(message);
        }

        public UniTask SendRoomDirectRelayAsync<T>(UID to, T message)
        {
            if (!isLocal)
            {
                Debug.LogError("SendRoomDirectAsync: !isLocal", this);
                return UniTask.CompletedTask;
            }
            return TinyServiceLocator.Resolve<GameNetworkController>().SendRoomDirectRelayAsync(to, message);
        }
    }
}
