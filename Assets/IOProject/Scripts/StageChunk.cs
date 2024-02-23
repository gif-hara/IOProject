using IOProject.ActorControllers;
using UnityEngine;

namespace IOProject
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class StageChunk : MonoBehaviour
    {
        public StageChunkModel Model { get; private set; }

        public void Setup(StageChunkModel model)
        {
            this.Model = model;
        }
    }
}
