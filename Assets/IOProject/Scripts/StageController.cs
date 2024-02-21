using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using IOProject.ActorControllers;
using R3;
using UnityEngine;

namespace IOProject
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class StageController
    {
        private StageChunk stageChunkPrefab;

        private Dictionary<Vector2Int, StageChunk> stageChunks = new();

        public StageController(StageChunk stageChunkPrefab)
        {
            this.stageChunkPrefab = stageChunkPrefab;
        }

        public void BeginGenerate(Actor actor)
        {
            var gameDesignData = TinyServiceLocator.Resolve<GameDesignData>();
            var range = gameDesignData.StageViewRange;
            var stageChunkSize = gameDesignData.StageChunkSize;
            for (var x = -range; x <= range; x++)
            {
                for (var y = -range; y <= range; y++)
                {
                    this.Generate(new Vector2Int(x, y), stageChunkSize);
                }
            }
        }

        private void Generate(Vector2Int positionId, int stageChunkSize)
        {
            if (stageChunks.ContainsKey(positionId))
            {
                return;
            }
            var stageChunk = Object.Instantiate(this.stageChunkPrefab);
            stageChunk.transform.position = new Vector3(positionId.x * stageChunkSize, 0, positionId.y * stageChunkSize);
            stageChunks.Add(positionId, stageChunk);
        }
    }
}
