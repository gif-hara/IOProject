using R3;
using UnityEngine;

namespace IOProject.ActorControllers
{
    /// <summary>
    /// アクターの姿勢を制御するインターフェイス
    /// </summary>
    public interface IActorPostureController
    {
        void Setup(Actor actor);

        void AddMove(Vector3 velocity);

        void AddRotate(Vector3 eulerAngle);

        ReadOnlyReactiveProperty<Vector2Int> PositionIdReactiveProperty { get; }

        void SyncPositionId(Vector2Int positionId);

        void SyncPosition(Vector3 position);

        void SyncRotation(Vector3 rotation);
    }
}
