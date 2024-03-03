using Cysharp.Threading.Tasks;
using R3;
using R3.Triggers;
using SoftGear.Strix.Client.Core;
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

        public UniTask SendRoomDirectRelayAsync<T>(UID to, T message)
        {
            if (!isLocal)
            {
                Debug.LogError("SendRoomDirectAsync: !isLocal", this);
                return UniTask.CompletedTask;
            }
            return TinyServiceLocator.Resolve<GameNetworkController>().SendRoomDirectRelayAsync(to, message);
        }

        private async UniTask StartAsRemote()
        {
            await UniTask.WaitUntil(() => this.strixReplicator.didStart);
            var remoteController = new ActorRemoteController();
            remoteController.Begin(actor);
        }
    }
}
