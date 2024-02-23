using System;
using System.Threading;
using R3;
using TMPro;
using UnityEngine;

namespace IOProject
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class StageChunk : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text occupiedNetworkIdText;

        public StageChunkModel Model { get; private set; }

        private CancellationTokenSource scope;

        public void Setup(StageChunkModel model)
        {
            scope?.Cancel();
            scope?.Dispose();
            scope = new CancellationTokenSource();
            this.Model = model;
            this.Model.OnAddDamageMap
                .Subscribe(networkInstanceId =>
                {
                    this.Model.DamageMap[networkInstanceId]
                        .Subscribe(damage =>
                        {
                            Debug.Log($"StageChunkModel.OnAddDamageMap: PositionId = {Model.PositionId}, networkInstanceId = {networkInstanceId}, damage = {damage}", this);
                        })
                        .RegisterTo(scope.Token);
                })
                .RegisterTo(scope.Token);
            this.Model.OccupiedNetworkId
                .Subscribe(networkInstanceId =>
                {
                    if (networkInstanceId == -1)
                    {
                        occupiedNetworkIdText.enabled = false;
                    }
                    else
                    {
                        occupiedNetworkIdText.enabled = true;
                        occupiedNetworkIdText.text = networkInstanceId.ToString();
                    }
                })
                .RegisterTo(scope.Token);
        }
    }
}
