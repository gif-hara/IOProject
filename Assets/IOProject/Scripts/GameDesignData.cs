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
        private int stageChunkDamageThreshold;
        public int StageChunkDamageThreshold => stageChunkDamageThreshold;
    }
}
