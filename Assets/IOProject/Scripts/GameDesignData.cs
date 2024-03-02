using UnityEngine;

namespace IOProject
{
    /// <summary>
    /// 
    /// </summary>
    [CreateAssetMenu(fileName = "GameDesignData", menuName = "IOProject/GameDesignData")]
    public sealed class GameDesignData : ScriptableObject
    {
        [SerializeField]
        private int stageChunkSize;
        public int StageChunkSize => stageChunkSize;

        [SerializeField]
        private int stageViewRange;
        public int StageViewRange => stageViewRange;

        [SerializeField]
        private int stageChunkDefaultDamageThreshold;

        /// <summary>
        /// デフォルトのステージチャンク占有ダメージ閾値
        /// </summary>
        public int StageChunkDefaultDamageThreshold => stageChunkDefaultDamageThreshold;

        [SerializeField]
        private int stageChunkOccupiedDamageThreshold;
        /// <summary>
        /// 占有されたステージチャンクのダメージ閾値
        /// </summary>
        public int StageChunkOccupiedDamageThreshold => stageChunkOccupiedDamageThreshold;

        [SerializeField]
        private int actorRemoteSendFrequency;
        /// <summary>
        /// アクターの秒間リモート送信頻度
        /// </summary>
        public int ActorRemoteSendFrequency => actorRemoteSendFrequency;

        [SerializeField]
        private Projectile projectilePrefab;
        /// <summary>
        /// プロジェクタイルのプレハブ
        /// </summary>
        public Projectile ProjectilePrefab => projectilePrefab;
    }
}
