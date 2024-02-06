using System.Threading;
using UnityEngine;

namespace IOProject
{
    /// <summary>
    /// Application.Quit時にUniTaskのCancellationTokenをキャンセルするためのコンポーネント
    /// </summary>
    public sealed class ApplicationQuitCancellationToken : MonoBehaviour
    {
        private static CancellationTokenSource source = null;

        public static CancellationToken Token
        {
            get
            {
                InitializeIfNeed();
                return source.Token;
            }
        }

        public static bool IsCancellationRequested
        {
            get
            {
                InitializeIfNeed();
                return source.IsCancellationRequested;
            }
        }

        private static void InitializeIfNeed()
        {
            if (source != null)
            {
                return;
            }

            source = new CancellationTokenSource();
            var gameObject = new GameObject(nameof(ApplicationQuitCancellationToken));
            DontDestroyOnLoad(gameObject);
            gameObject.AddComponent<ApplicationQuitCancellationToken>();
        }

        private void OnApplicationQuit()
        {
            source.Cancel();
            source.Dispose();
            source = null;
            Debug.Log("ApplicationQuitCancellationTokenSource.OnApplicationQuit");
        }
    }
}
