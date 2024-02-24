using System.Collections.Generic;
using IOProject.ActorControllers;
using R3;
using SoftGear.Strix.Client.Core;
using SoftGear.Strix.Net;
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

        private StrixMessageStageChunkModelDictionary strixMessageStageChunkModels = new();

        private Dictionary<Vector2Int, StageChunk> stageChunks = new();

        private Dictionary<Vector2Int, StageChunkModel> stageChunkModels = new();

        private bool isFirstDeserialize = true;

        public bool IsReady
        {
            get
            {
                if (isLocal)
                {
                    return true;
                }

                return !isFirstDeserialize;
            }
        }

        void Awake()
        {
            TinyServiceLocator.RegisterAsync(this, destroyCancellationToken).Forget();
        }

        void Start()
        {
            if (!StrixNetwork.instance.isRoomOwner)
            {
                RpcToRoomOwner(nameof(RequestInitialize), StrixNetwork.instance.selfRoomMember.GetUid());
                Debug.Log("Start");
            }
        }

        [StrixRpc]
        private void RequestInitialize(UID uid)
        {
            Rpc(uid, nameof(Initialize), this.strixMessageStageChunkModels);
            Debug.Log("RequestInitialize");
        }

        [StrixRpc]
        private void Initialize(StrixMessageStageChunkModelDictionary strixMessageStageChunkModels)
        {
            this.strixMessageStageChunkModels = strixMessageStageChunkModels;
            foreach (var (positionId, stageChunkModel) in strixMessageStageChunkModels)
            {
                stageChunkModels.Add(positionId, new StageChunkModel(stageChunkModel));
            }
            isFirstDeserialize = false;
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
            var message = new StrixMessageStageChunkModel();
            stageChunkModel = new StageChunkModel(positionId, message);
            stageChunkModels.Add(positionId, stageChunkModel);
            strixMessageStageChunkModels.Add(positionId, message);
            return stageChunkModel;
        }
    }
}
