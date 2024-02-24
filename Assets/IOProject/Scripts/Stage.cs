using System.Collections.Generic;
using IOProject.ActorControllers;
using R3;
using SoftGear.Strix.Unity.Runtime;
using UnityEngine;

namespace IOProject
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Stage : StrixBehaviour
    {
        [SerializeField]
        private StageChunk stageChunkPrefab;

        private Dictionary<Vector2Int, StageChunk> stageChunks = new();

        private Dictionary<Vector2Int, StageChunkModel> stageChunkModels = new();

        void Awake()
        {
            TinyServiceLocator.RegisterAsync(this, destroyCancellationToken).Forget();
        }

        public void Begin(Actor actor)
        {
            var gameDesignData = TinyServiceLocator.Resolve<GameDesignData>();
            var range = gameDesignData.StageViewRange;
            var actorPositionId = actor.PostureController.PositionIdReactiveProperty.CurrentValue;
            var stageChunkSize = gameDesignData.StageChunkSize;
            for (var x = -range; x <= range; x++)
            {
                for (var y = -range; y <= range; y++)
                {
                    var stageChunkPositionId = new Vector2Int(x, y);
                    var stageChunk = Instantiate(this.stageChunkPrefab);
                    stageChunk.transform.position = new Vector3(stageChunkPositionId.x * stageChunkSize, 0, stageChunkPositionId.y * stageChunkSize);
                    stageChunk.Setup(GetOrCreateStageChunkModel(actorPositionId + stageChunkPositionId));
                    stageChunks.Add(stageChunkPositionId, stageChunk);
                }
            }
            actor.PostureController.PositionIdReactiveProperty
                .Subscribe(positionId =>
                {
                    for (var x = -range; x <= range; x++)
                    {
                        for (var y = -range; y <= range; y++)
                        {
                            var stagePositionId = new Vector2Int(x, y);
                            var fixedPositionId = positionId + stagePositionId;
                            stageChunks[stagePositionId].Setup(GetOrCreateStageChunkModel(fixedPositionId));
                        }
                    }
                });
        }

        public void TakeDamageStageChunk(long attackerNetworkInstanceId, Vector2Int positionId, int damage)
        {
            var model = GetOrCreateStageChunkModel(positionId);
            model.AddDamageMap(attackerNetworkInstanceId, damage);
        }

        private StageChunkModel GetOrCreateStageChunkModel(Vector2Int positionId)
        {
            if (stageChunkModels.TryGetValue(positionId, out var stageChunkModel))
            {
                return stageChunkModel;
            }
            stageChunkModel = new StageChunkModel(positionId);
            stageChunkModels.Add(positionId, stageChunkModel);
            return stageChunkModel;
        }
    }
}
