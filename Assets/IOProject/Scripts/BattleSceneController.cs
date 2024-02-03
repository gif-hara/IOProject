using HK.Framework.BootSystems;
using IOProject.ActorControllers;
using UnityEngine;

namespace IOProject
{
    public class BattleSceneController : SceneController
    {
        [SerializeField]
        private Actor playerActorPrefab;

        [SerializeField]
        private GameCameraController gameCameraControllerPrefab;

        async void Start()
        {
            await BootSystem.IsReady;
            TinyServiceLocator.RegisterAsync<IInputController>(new InputController(), this.destroyCancellationToken).Forget();
            var playerActor = Instantiate(this.playerActorPrefab);
            var gameCameraController = Instantiate(this.gameCameraControllerPrefab);
            gameCameraController.SetTarget(playerActor.transform);
            var playerActorController = new PlayerActorController();
            playerActorController.Attach(playerActor);
        }
    }
}
