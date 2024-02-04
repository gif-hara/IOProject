using Cinemachine;
using UnityEngine;

namespace IOProject
{
    public class GameCameraController : MonoBehaviour
    {
        [SerializeField]
        private CinemachineVirtualCamera virtualCamera;

        public void SetFollow(Transform follow)
        {
            this.virtualCamera.Follow = follow;
        }

        public void SetLookAt(Transform lookAt)
        {
            this.virtualCamera.LookAt = lookAt;
        }
    }
}
