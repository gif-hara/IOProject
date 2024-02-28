using UnityEngine;

namespace IOProject
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class NetworkMessage
    {
        public sealed class Helloworld
        {
            public string message;

            override public string ToString()
            {
                return $"message: {message}";
            }
        }

        public struct UpdateActorPosition
        {
            public Vector3 position;
        }

        public struct UpdateActorPositionId
        {
            public Vector2Int positionId;
        }

        public struct UpdateActorRotation
        {
            public Vector3 rotation;
        }
    }
}
