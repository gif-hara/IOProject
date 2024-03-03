using SoftGear.Strix.Client.Core;
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
            public Vector2Int positionId;
        }

        public struct UpdateActorRotation
        {
            public Vector3 rotation;
        }

        public struct UpdateCanFire
        {
            public bool canFire;
        }

        public struct UpdateHitPointMax
        {
            public int hitPointMax;
        }

        public struct UpdateHitPoint
        {
            public int hitPoint;
        }

        public struct GiveDamageActor
        {
            public UID target;

            public int damage;
        }
    }
}
