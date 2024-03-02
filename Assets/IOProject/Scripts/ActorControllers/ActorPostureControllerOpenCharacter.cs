using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Cysharp.Threading.Tasks.Triggers;
using R3;
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

        private Actor actor;

        private Vector3 velocity;

        public float RotationX { get; private set; }

        public float RotationY { get; private set; }

        private ReactiveProperty<Vector2Int> positionIdReactiveProperty = new ReactiveProperty<Vector2Int>();

        public ReadOnlyReactiveProperty<Vector2Int> PositionIdReactiveProperty => positionIdReactiveProperty;

        public void Setup(Actor actor)
        {
            this.actor = actor;
            actor.GetAsyncUpdateTrigger()
                .Subscribe(_ =>
                {
                    if (velocity != Vector3.zero)
                    {
                        characterController.Move(velocity);
                        var chunkSize = TinyServiceLocator.Resolve<GameDesignData>().StageChunkSize;
                        var newPositionId = positionIdReactiveProperty.Value + new Vector2Int(Mathf.FloorToInt(transform.position.x / chunkSize), Mathf.FloorToInt(transform.position.z / chunkSize));
                        if (positionIdReactiveProperty.Value != newPositionId)
                        {
                            var diff = newPositionId - positionIdReactiveProperty.Value;
                            characterController.transform.position -= new Vector3(diff.x * chunkSize, 0, diff.y * chunkSize);
                            positionIdReactiveProperty.Value = newPositionId;
                        }
                        velocity = Vector3.zero;
                    }
                })
                .AddTo(actor.destroyCancellationToken);
        }

        public void AddMove(Vector3 velocity)
        {
            this.velocity += velocity;
        }

        public void AddRotate(Vector3 eulerAngle)
        {
            if (eulerAngle == Vector3.zero)
            {
                return;
            }
            RotationX += eulerAngle.x;
            RotationY += eulerAngle.y;
            RotationX = Mathf.Clamp(RotationX, -89, 89);
            RotationY = Mathf.Repeat(RotationY, 360);
            foreach (var t in rotationXTransforms)
            {
                t.localEulerAngles = new Vector3(RotationX, 0, 0);
            }
            foreach (var t in rotationYTransforms)
            {
                t.localEulerAngles = new Vector3(0, RotationY, 0);
            }
        }

        public void SyncPositionId(Vector2Int positionId)
        {
            positionIdReactiveProperty.Value = positionId;
        }

        public void SyncPosition(Vector3 position)
        {
            characterController.transform.position = position;
        }

        public void SyncRotation(Vector3 rotation)
        {
            RotationX = rotation.x;
            RotationY = rotation.y;
            foreach (var t in rotationXTransforms)
            {
                t.localEulerAngles = new Vector3(RotationX, 0, 0);
            }
            foreach (var t in rotationYTransforms)
            {
                t.localEulerAngles = new Vector3(0, RotationY, 0);
            }
        }
    }
}
