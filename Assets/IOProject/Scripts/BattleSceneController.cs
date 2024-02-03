using HK.Framework.BootSystems;
using UnityEngine;

namespace IOProject
{
    public class BattleSceneController : SceneController
    {
        async void Start()
        {
            await BootSystem.IsReady;
        }
    }
}
