using Cysharp.Threading.Tasks;
using UnityEngine;

namespace IOProject.ActorControllers
{
    public class ActorModel
    {
        private readonly AsyncReactiveProperty<Vector2> position = new(Vector2.zero);

        private readonly AsyncReactiveProperty<Quaternion> rotation = new(Quaternion.identity);

        public Vector2 Position
        {
            get => position.Value;
            set => position.Value = value;
        }

        public Quaternion Rotation
        {
            get => rotation.Value;
            set => rotation.Value = value;
        }

        public IAsyncReactiveProperty<Vector2> PositionAsyncReactiveProperty => position;

        public IAsyncReactiveProperty<Quaternion> RotationAsyncReactiveProperty => rotation;
    }
}
