using System.Collections.Generic;
using IOProject.ActorControllers;
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

        private Dictionary<Vector2Int, StageChunkModel> stageChunkModels = new();

        public StageController(StageChunk stageChunkPrefab)
        {
            this.stageChunkPrefab = stageChunkPrefab;
        }

        public void BeginGenerate(Actor actor)
        {
            var gameDesignData = TinyServiceLocator.Resolve<GameDesignData>();
            var range = gameDesignData.StageViewRange;
            var actorPositionId = actor.PostureController.PositionIdReactiveProperty.CurrentValue;
            for (var x = -range; x <= range; x++)
            {
                for (var y = -range; y <= range; y++)
                {
                    Generate(actorPositionId, new Vector2Int(x, y), gameDesignData.StageChunkSize);
                }
            }
        }

        public void TakeDamageStageChunk(Vector2Int positionId, int damage)
        {
            var model = GetOrCreateStageChunkModel(positionId);
            model.AddDamageMap(0, damage);
        }

        private void Generate(Vector2Int actorPositionId, Vector2Int stageChunkPositionId, int stageChunkSize)
        {
            if (stageChunks.ContainsKey(stageChunkPositionId))
            {
                return;
            }
            var stageChunk = Object.Instantiate(this.stageChunkPrefab);
            stageChunk.transform.position = new Vector3(stageChunkPositionId.x * stageChunkSize, 0, stageChunkPositionId.y * stageChunkSize);
            stageChunk.Setup(GetOrCreateStageChunkModel(actorPositionId + stageChunkPositionId));
            stageChunks.Add(stageChunkPositionId, stageChunk);
        }

        private StageChunkModel GetOrCreateStageChunkModel(Vector2Int positionId)
        {
            if (stageChunkModels.TryGetValue(positionId, out var stageChunkModel))
            {
                return stageChunkModel;
            }
            stageChunkModel = new StageChunkModel();
            stageChunkModels.Add(positionId, stageChunkModel);
            return stageChunkModel;
        }
    }
}
