using HK.Framework.MessageSystems;
using MessagePipe;

namespace IOProject.ActorControllers
{
    /// <summary>
    /// 
    /// </summary>
    public static class ActorEvents
    {
        public sealed class OnSpawned : Message<OnSpawned, Actor>
        {
            public Actor Actor => this.Param1;
        }

        public static void RegisterEvents(BuiltinContainerBuilder builder)
        {
            builder.AddMessageBroker<OnSpawned>();
        }
    }
}
