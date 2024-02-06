using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace IOProject
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class DebugUI
    {
        private UIDocument debugUI;

        private VisualTreeAsset logElementPrefab;

        public DebugUI(UIDocument debugUIPrefab, VisualTreeAsset logElementPrefab)
        {
            this.SetupAsync(debugUIPrefab, logElementPrefab).Forget();
        }

        private async UniTask SetupAsync(UIDocument debugUIPrefab, VisualTreeAsset logElementPrefab)
        {
            this.debugUI = Object.Instantiate(debugUIPrefab);
            Object.DontDestroyOnLoad(this.debugUI);
            this.logElementPrefab = logElementPrefab;
            Application.logMessageReceived += this.OnLogMessageReceived;
            await UniTask.WaitUntilCanceled(ApplicationQuitCancellationToken.Token);
            Application.logMessageReceived -= this.OnLogMessageReceived;
        }

        private void OnLogMessageReceived(string condition, string stackTrace, LogType type)
        {
            if (ApplicationQuitCancellationToken.IsCancellationRequested)
            {
                return;
            }
            if (this.debugUI == null)
            {
                return;
            }
            var logElement = this.logElementPrefab.CloneTree();
            logElement.Q<Label>("Message").text = condition;
            this.debugUI.rootVisualElement.Q<ScrollView>("LogArea").Add(logElement);
        }
    }
}
