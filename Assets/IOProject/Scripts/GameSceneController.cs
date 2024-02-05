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
        private PlayerSpec playerSpec;

        [SerializeField]
        private Actor playerActorPrefab;

        [SerializeField]
        private GameCameraController gameCameraControllerPrefab;

        [SerializeField]
        private GameObject stageChunkPrefab;

        async void Start()
        {
            await HK.Framework.BootSystems.BootSystem.IsReady;
            ConnectMasterServer();
            TinyServiceLocator.RegisterAsync<IInputController>(new InputController(), this.destroyCancellationToken).Forget();
            TinyServiceLocator.Resolve<IInputController>().SetCursorVisibliity(false);
            var playerActor = playerActorPrefab.Spawn(new ActorModel());
            var gameCameraController = Instantiate(this.gameCameraControllerPrefab);
            gameCameraController.SetFollow(playerActor.LocatorController.GetLocator("View.FirstPerson.Follow"));
            gameCameraController.SetLookAt(playerActor.LocatorController.GetLocator("View.FirstPerson.LookAt"));
            var playerActorController = new PlayerActorController();
            playerActorController.Attach(playerActor, playerSpec);
            Instantiate(this.stageChunkPrefab);
        }

        private void ConnectMasterServer()
        {
            LogManager.Instance.Filter = Level.INFO;
            StrixNetwork.instance.applicationId = "686bc912-86ae-4134-a39c-cb4884d95eff";
            StrixNetwork.instance.ConnectMasterServer("63da7b783afd86c8787cffbf.game.strixcloud.net", 9122,
                args =>
                {
                    Debug.Log($"connectEventHandler: {args}");
                    JoinRandomRoom();
                },
                args =>
                {
                    Debug.Log($"errorEventHandler: {args}");
                }
            );
        }

        private void JoinRandomRoom()
        {
            StrixNetwork.instance.JoinRandomRoom("Test",
                args =>
                {
                    Debug.Log($"joinRoomEventHandler: {args}");
                },
                args =>
                {
                    Debug.Log($"failreRoomEventHandler: {args}");
                    CreateRoom();
                }
            );
        }

        private void CreateRoom()
        {
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
                },
                args =>
                {
                    Debug.Log($"failureCreateRoomEventHandler: {args}");
                }
            );
        }
    }
}
