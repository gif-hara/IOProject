using Cinemachine;
using UnityEngine;

namespace IOProject
{
    public class GameCameraController : MonoBehaviour
    {
        [SerializeField]
        private CinemachineVirtualCamera virtualCamera;

        public void SetTarget(Transform target)
        {
            this.virtualCamera.Follow = target;
            this.virtualCamera.LookAt = target;
        }
    }
}
