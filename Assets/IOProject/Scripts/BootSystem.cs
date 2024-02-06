using Cysharp.Threading.Tasks;
using HK.Framework;
using IOProject.ActorControllers;
using UnityEngine;
using UnityEngine.UIElements;

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

            HK.Framework.BootSystems.BootSystem.AdditionalSetupAsync += async () =>
            {
                await SetupDebugUIAsync();
            };
        }

        private static async UniTask SetupDebugUIAsync()
        {
            var result = await UniTask.WhenAll
            (
                AssetLoader.LoadAsync<GameObject>("Assets/IOProject/Prefabs/UI.Debug.prefab"),
                AssetLoader.LoadAsync<VisualTreeAsset>("Assets/IOProject/UI/Log.Element.uxml")
            );

            var debugUI = result.Item1.GetComponent<UIDocument>();
            var logElementPrefab = result.Item2;
            new DebugUI(debugUI, logElementPrefab);
        }
    }
}
