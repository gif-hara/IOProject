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

        void AddRotate(Quaternion rotation);
    }
}
