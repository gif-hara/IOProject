using R3;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace IOProject.ActorControllers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ActorRemoteController
    {
        private Vector3 position;

        public void Begin(Actor actor)
        {
            TinyServiceLocator.Resolve<GameNetworkController>()
                .RoomRelayAsObservable()
                .WhereOwner(actor.NetworkController)
                .MatchMessage<NetworkMessage.UpdateActorPosition>()
                .Subscribe(x =>
                {
                    var diff = x.message.position - actor.transform.position;
                    if (diff.magnitude > 5.0f)
                    {
                        actor.PostureController.Warp(x.message.position);
                        position = x.message.position;
                    }
                    else
                    {
                        actor.PostureController.AddMove(diff);
                        position += diff;
                    }
                })
                .RegisterTo(actor.destroyCancellationToken);
        }
    }
}
