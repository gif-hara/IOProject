using UnityEngine;

namespace IOProject
{
    /// <summary>
    /// 
    /// </summary>
    [CreateAssetMenu(menuName = "IOProject/PlayerSpec")]
    public sealed class PlayerSpec : ScriptableObject
    {
        public float moveSpeed = 5.0f;

        public float rotateSpeed = 5.0f;

        public Projectile projectilePrefab;

        public float fireCoolTime;
    }
}
