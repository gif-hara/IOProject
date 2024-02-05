using UnityEngine;

namespace IOProject
{
    public class InputController : IInputController
    {
        private readonly InputActions actions = new();

        public InputActions Actions => actions;

        public InputController()
        {
            actions.Enable();
        }

        public void SetCursorVisibliity(bool isVisible)
        {
            Cursor.visible = isVisible;
            Cursor.lockState = isVisible ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}
