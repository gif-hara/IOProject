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
    }
}
