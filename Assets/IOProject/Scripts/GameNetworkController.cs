using Cysharp.Threading.Tasks;
using R3;
using SoftGear.Strix.Client.Core;
using SoftGear.Strix.Client.Room.Message;
using SoftGear.Strix.Net.Logging;
using SoftGear.Strix.Net.Serialization;
using SoftGear.Strix.Unity.Runtime;
using UnityEngine;

namespace IOProject
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GameNetworkController
    {
        private readonly Subject<NotificationEventArgs<RoomRelayNotification>> roomRelaySubject = new();
        public Observable<NotificationEventArgs<RoomRelayNotification>> RoomRelayAsObservable() => this.roomRelaySubject;

        private readonly Subject<NotificationEventArgs<RoomDirectRelayNotification>> roomDirectRelaySubject = new();
        public Observable<NotificationEventArgs<RoomDirectRelayNotification>> RoomDirectRelayAsObservable() => this.roomDirectRelaySubject;

        public async UniTask ConnectAsync()
        {
            ObjectFactory.Instance.Register(typeof(StrixMessageStageChunkModel));
            ObjectFactory.Instance.Register(typeof(StrixMessageStageChunkModelDictionary));
            ObjectFactory.Instance.Register(typeof(DamageDictionary));
            ObjectFactory.Instance.Register(typeof(NetworkMessage.Helloworld));
            ObjectFactory.Instance.Register(typeof(NetworkMessage.UpdateActorPosition));
            ObjectFactory.Instance.Register(typeof(NetworkMessage.UpdateActorRotation));
            ObjectFactory.Instance.Register(typeof(NetworkMessage.UpdateCanFire));
            ObjectFactory.Instance.Register(typeof(NetworkMessage.UpdateHitPointMax));
            ObjectFactory.Instance.Register(typeof(NetworkMessage.UpdateHitPoint));
            ObjectFactory.Instance.Register(typeof(NetworkMessage.GiveDamageActor));
            await ConnectMasterServerAsync();
            StrixNetwork.instance.roomSession.roomClient.RoomRelayNotified += OnRoomRelayNotified;
            StrixNetwork.instance.roomSession.roomClient.RoomDirectRelayNotified += OnRoomDirectRelayNotified;
        }

        public UniTask SendRoomRelayAsync<T>(T message)
        {
            var source = new UniTaskCompletionSource();
            StrixNetwork.instance.SendRoomRelay(
                message,
                args =>
                {
                    source.TrySetResult();
                },
                args =>
                {
                    source.TrySetException(new System.Exception($"SendRoomRelayAsync: {args}, message = {message}, cause = {args.cause}, session = {args.session}"));
                });
            return source.Task;
        }

        public UniTask SendRoomDirectRelayAsync<T>(UID to, T message)
        {
            var source = new UniTaskCompletionSource();
            StrixNetwork.instance.SendRoomDirectRelay(
                to,
                message,
                args =>
                {
                    source.TrySetResult();
                },
                args =>
                {
                    source.TrySetException(new System.Exception($"SendRoomDirectRelayAsync: {args}, message = {message}, cause = {args.cause}, session = {args.session}"));
                });
            return source.Task;
        }

        private async UniTask ConnectMasterServerAsync()
        {
            var source = new UniTaskCompletionSource();
            LogManager.Instance.Filter = Level.INFO;
            StrixNetwork.instance.applicationId = "686bc912-86ae-4134-a39c-cb4884d95eff";
            StrixNetwork.instance.ConnectMasterServer("wss://63da7b783afd86c8787cffbf.game.strixcloud.net:9122",
                async args =>
                {
                    Debug.Log($"connectEventHandler: {args}");
                    await JoinRandomRoomAsync();
                    source.TrySetResult();
                },
                args =>
                {
                    Debug.Log($"errorEventHandler: {args}");
                    source.TrySetException(new System.Exception(args.ToString()));
                }
            );

            await source.Task;
        }

        private async UniTask JoinRandomRoomAsync()
        {
            var source = new UniTaskCompletionSource();
            StrixNetwork.instance.JoinRandomRoom("Test",
                args =>
                {
                    Debug.Log($"joinRoomEventHandler: {args}");
                    source.TrySetResult();
                },
                async args =>
                {
                    Debug.Log($"failreRoomEventHandler: {args}");
                    await CreateRoomAsync();
                    source.TrySetResult();
                }
            );

            await source.Task;
        }

        private async UniTask CreateRoomAsync()
        {
            var source = new UniTaskCompletionSource();
            var roomProperties = new RoomProperties
            {
                capacity = 100,
                name = "TestRoom",
            };
            var roomMemberProperties = new RoomMemberProperties
            {
                name = "Test",
            };
            StrixNetwork.instance.CreateRoom(roomProperties, roomMemberProperties,
                args =>
                {
                    Debug.Log($"createRoomEventHandler: {args}");
                    source.TrySetResult();
                },
                args =>
                {
                    Debug.Log($"failureCreateRoomEventHandler: {args}");
                    source.TrySetException(new System.Exception(args.ToString()));
                }
            );

            await source.Task;
        }

        private void OnRoomRelayNotified(NotificationEventArgs<RoomRelayNotification> notification)
        {
            roomRelaySubject.OnNext(notification);
        }

        private void OnRoomDirectRelayNotified(NotificationEventArgs<RoomDirectRelayNotification> notification)
        {
            roomDirectRelaySubject.OnNext(notification);
        }
    }
}
