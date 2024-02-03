using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine.InputSystem;

namespace MH2
{
    /// <summary>
    /// 
    /// </summary>
    public static partial class Extensions
    {
        public static IUniTaskAsyncEnumerable<InputAction.CallbackContext> OnPerformedAsync(this InputAction self)
        {
            return UniTaskAsyncEnumerable.Create<InputAction.CallbackContext>(async (writer, token) =>
            {
                void OnPerformed(InputAction.CallbackContext context)
                {
                    writer.YieldAsync(context);
                }
                self.performed += OnPerformed;
                await UniTask.WaitUntilCanceled(token);
                self.performed -= OnPerformed;
            });
        }

        public static IUniTaskAsyncEnumerable<InputAction.CallbackContext> OnCanceledAsync(this InputAction self)
        {
            return UniTaskAsyncEnumerable.Create<InputAction.CallbackContext>(async (writer, token) =>
            {
                void OnCanceled(InputAction.CallbackContext context)
                {
                    writer.YieldAsync(context);
                }
                self.canceled += OnCanceled;
                await UniTask.WaitUntilCanceled(token);
                self.canceled -= OnCanceled;
            });
        }
    }
}
