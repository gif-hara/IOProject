using System;
using System.Threading;
using R3;
using UnityEngine;

namespace IOProject
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class StageChunk : MonoBehaviour
    {
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
            this.Model.OnOccupied
                .Subscribe(networkInstanceId =>
                {
                    Debug.Log($"StageChunkModel.OnOccupied: PositionId = {Model.PositionId}, networkInstanceId = {networkInstanceId}", this);
                })
                .RegisterTo(scope.Token);
        }
    }
}
