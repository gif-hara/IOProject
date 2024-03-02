using Cysharp.Threading.Tasks;
using R3;
using R3.Triggers;
using SoftGear.Strix.Unity.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace IOProject.ActorControllers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ActorNetworkController : StrixBehaviour
    {
        [SerializeField]
        private Actor actor;

        void Awake()
        {
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
                .WhereOwner(this)
                .MatchMessage<NetworkMessage.Helloworld>()
                .Subscribe(x =>
                {
                    Debug.Log($"RoomRelayAsObservable: {x.message}", this);
                })
                .RegisterTo(this.destroyCancellationToken);
        }

        void Start()
        {
            StartAsRemote().Forget();
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

        private async UniTask StartAsRemote()
        {
            await UniTask.WaitUntil(() => this.strixReplicator.didStart);
            var remoteController = new ActorRemoteController();
            remoteController.Begin(actor);
        }
    }
}
