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

        public sealed class UpdateActorPosition
        {
            public Vector3 position;
        }

        public sealed class UpdateActorPositionId
        {
            public Vector2Int positionId;
        }
    }
}
