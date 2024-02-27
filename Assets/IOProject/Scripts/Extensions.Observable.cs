using R3;
using SoftGear.Strix.Client.Core;
using SoftGear.Strix.Client.Room.Message;
using SoftGear.Strix.Unity.Runtime;

namespace IOProject
{
    /// <summary>
    /// 
    /// </summary>
    public static partial class Extensions
    {
        public static Observable<NotificationEventArgs<RoomRelayNotification>> WhereOwner(
            this Observable<NotificationEventArgs<RoomRelayNotification>> self,
            StrixBehaviour strixBehaviour
            )
        {
            return self.Where(x => x.Data.GetFromUid().Equals(strixBehaviour.strixReplicator.ownerUid));
        }

        public static Observable<(NotificationEventArgs<RoomRelayNotification> notification, T message)> MatchMessage<T>(
            this Observable<NotificationEventArgs<RoomRelayNotification>> self
            )
        {
            return self
                .Where(x => x.Data.GetMessageToRelay() is T)
                .Select(x => (x, (T)x.Data.GetMessageToRelay()));
        }
    }
}
