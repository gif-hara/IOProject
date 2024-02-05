using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Cysharp.Threading.Tasks.Triggers;
using StandardAssets.Characters.Physics;
using UnityEngine;

namespace IOProject.ActorControllers
{
    /// <summary>
    /// キャラクターの姿勢を制御するクラス
    /// </summary>
    public sealed class ActorPostureControllerOpenCharacter : MonoBehaviour, IActorPostureController
    {
        [SerializeField]
        private OpenCharacterController characterController;

        [SerializeField]
        private List<Transform> rotationXTransforms;

        [SerializeField]
        private List<Transform> rotationYTransforms;

        private Vector3 velocity;

        private float rotationX;

        private float rotationY;

        public void Setup(Actor actor)
        {
            actor.GetAsyncUpdateTrigger()
                .Subscribe(_ =>
                {
                    if (velocity != Vector3.zero)
                    {
                        characterController.Move(velocity);
                        velocity = Vector3.zero;
                    }
                })
                .AddTo(actor.destroyCancellationToken);
        }

        public void AddMove(Vector3 velocity)
        {
            this.velocity += velocity;
        }

        public void AddRotate(Quaternion rotation)
        {
            var euler = rotation.eulerAngles;
            rotationX += euler.x;
            rotationY += euler.y;
            // rotationX = Mathf.Clamp(rotationX, -90, 90);
            // rotationY = Mathf.Repeat(rotationY, 360);
            foreach (var t in rotationXTransforms)
            {
                t.localEulerAngles = new Vector3(rotationX, 0, 0);
            }
            foreach (var t in rotationYTransforms)
            {
                t.localEulerAngles = new Vector3(0, rotationY, 0);
            }
        }
    }
}
