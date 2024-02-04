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

        [SerializeField]
        private GameObject stageChunkPrefab;

        async void Start()
        {
            await BootSystem.IsReady;
            TinyServiceLocator.RegisterAsync<IInputController>(new InputController(), this.destroyCancellationToken).Forget();
            var playerActor = playerActorPrefab.Spawn(new ActorModel());
            var gameCameraController = Instantiate(this.gameCameraControllerPrefab);
            gameCameraController.SetFollow(playerActor.LocatorController.GetLocator("View.FirstPerson.Follow"));
            gameCameraController.SetLookAt(playerActor.LocatorController.GetLocator("View.FirstPerson.LookAt"));
            var playerActorController = new PlayerActorController();
            playerActorController.Attach(playerActor);
            Instantiate(this.stageChunkPrefab);
        }
    }
}
