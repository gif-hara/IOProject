using IOProject.ActorControllers;
using UnityEngine;

namespace IOProject
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class BootSystem
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            HK.Framework.BootSystems.BootSystem.AdditionalSetupContainerBuilderAsync += builder =>
            {
                ActorEvents.RegisterEvents(builder);
            };
        }
    }
}
