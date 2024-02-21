using Cysharp.Threading.Tasks;
using IOProject.ActorControllers;
using SoftGear.Strix.Net.Logging;
using SoftGear.Strix.Unity.Runtime;
using UnityEngine;

namespace IOProject
{
    public class GameSceneController : SceneController
    {
        [SerializeField]
        private GameDesignData gameDesignData;

        [SerializeField]
        private PlayerSpec playerSpec;

        [SerializeField]
        private Actor playerActorPrefab;

        [SerializeField]
        private GameCameraController gameCameraControllerPrefab;

        [SerializeField]
        private StageChunk stageChunkPrefab;

        [SerializeField]
        private HKUIDocument reticleUIPrefab;

        async void Start()
        {
            await HK.Framework.BootSystems.BootSystem.IsReady;
            await ConnectMasterServerAsync();
            TinyServiceLocator.RegisterAsync(this.gameDesignData, this.destroyCancellationToken).Forget();
            TinyServiceLocator.RegisterAsync<IInputController>(new InputController(), this.destroyCancellationToken).Forget();
            TinyServiceLocator.Resolve<IInputController>().SetCursorVisibliity(false);
            var playerActor = playerActorPrefab.Spawn();
            TinyServiceLocator.RegisterAsync("LocalActor", playerActor, this.destroyCancellationToken).Forget();
            var gameCameraController = Instantiate(this.gameCameraControllerPrefab);
            gameCameraController.SetFollow(playerActor.LocatorController.GetLocator("View.FirstPerson.Follow"));
            gameCameraController.SetLookAt(playerActor.LocatorController.GetLocator("View.FirstPerson.LookAt"));
            var playerActorController = new PlayerActorController();
            playerActorController.Attach(playerActor, playerSpec);
            var stageController = new StageController(stageChunkPrefab);
            stageController.BeginGenerate(playerActor);
            Instantiate(this.stageChunkPrefab);
            var reticleUI = Instantiate(this.reticleUIPrefab);
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
    }
}
