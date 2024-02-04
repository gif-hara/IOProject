using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using IOProject.ActorControllers;
using UnityEngine;

namespace IOProject
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ActorPostureController
    {
        public ActorPostureController(
            Actor actor,
            List<Transform> positionTransforms,
            List<Transform> rotationXTransforms,
            List<Transform> rotationYTransforms
            )
        {
            actor.Model.PositionAsyncReactiveProperty
                .Subscribe(position =>
                {
                    foreach (var positionTransform in positionTransforms)
                    {
                        positionTransform.localPosition = new Vector3(position.x, 0.0f, position.y);
                    }
                })
                .AddTo(actor.destroyCancellationToken);
            actor.Model.RotationAsyncReactiveProperty
                .Subscribe(rotation =>
                {
                    foreach (var rotationXTransform in rotationXTransforms)
                    {
                        rotationXTransform.localRotation = Quaternion.Euler(rotation.eulerAngles.x, 0.0f, 0.0f);
                    }
                    foreach (var rotationYTransform in rotationYTransforms)
                    {
                        rotationYTransform.localRotation = Quaternion.Euler(0.0f, rotation.eulerAngles.y, 0.0f);
                    }
                })
                .AddTo(actor.destroyCancellationToken);
        }
    }
}
