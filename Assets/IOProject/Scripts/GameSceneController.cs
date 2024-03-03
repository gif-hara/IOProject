using Cysharp.Threading.Tasks;
using IOProject.ActorControllers;
using SoftGear.Strix.Net.Logging;
using SoftGear.Strix.Net.Serialization;
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
        private Stage stagePrefab;

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
            var gameNetworkController = new GameNetworkController();
            await gameNetworkController.ConnectAsync();
            TinyServiceLocator.RegisterAsync(gameNetworkController).Forget();
            TinyServiceLocator.RegisterAsync(this.gameDesignData, this.destroyCancellationToken).Forget();
            TinyServiceLocator.RegisterAsync<IInputController>(new InputController(), this.destroyCancellationToken).Forget();
            TinyServiceLocator.Resolve<IInputController>().SetCursorVisibliity(false);
            TinyServiceLocator.RegisterAsync(new ActorManager(), this.destroyCancellationToken).Forget();

            if (StrixNetwork.instance.isRoomOwner)
            {
                var _stage = Instantiate(this.stagePrefab);
            }

            await UniTask.WaitUntil(() => TinyServiceLocator.Contains<Stage>());
            var stage = TinyServiceLocator.Resolve<Stage>();
            await UniTask.WaitUntil(() => stage.IsReady);
            var playerActor = playerActorPrefab.Spawn();
            TinyServiceLocator.RegisterAsync("LocalActor", playerActor, this.destroyCancellationToken).Forget();
            var gameCameraController = Instantiate(this.gameCameraControllerPrefab);
            gameCameraController.SetFollow(playerActor.LocatorController.GetLocator("View.FirstPerson.Follow"));
            gameCameraController.SetLookAt(playerActor.LocatorController.GetLocator("View.FirstPerson.LookAt"));
            var playerActorController = new PlayerActorController();
            playerActorController.Attach(playerActor, playerSpec);
            stage.Begin(playerActor);
            var reticleUI = Instantiate(this.reticleUIPrefab);
        }

    }
}
