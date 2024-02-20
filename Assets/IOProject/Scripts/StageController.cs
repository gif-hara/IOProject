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
        private GameObject stageChunkPrefab;

        private Dictionary<Vector2Int, GameObject> stageChunks = new();

        public StageController(GameObject stageChunkPrefab)
        {
            this.stageChunkPrefab = stageChunkPrefab;
        }

        public void BeginGenerate(Actor actor)
        {
            const int range = 10;
            for (var x = -range; x <= range; x++)
            {
                for (var y = -range; y <= range; y++)
                {
                    this.Generate(new Vector2Int(x, y));
                }
            }
        }

        private void Generate(Vector2Int positionId)
        {
            if (stageChunks.ContainsKey(positionId))
            {
                return;
            }
            var stageChunk = Object.Instantiate(this.stageChunkPrefab);
            stageChunk.transform.position = new Vector3(positionId.x * 50, 0, positionId.y * 50);
            stageChunks.Add(positionId, stageChunk);
        }
    }
}
