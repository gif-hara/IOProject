using System.Collections.Generic;
using UnityEngine;
using UnitySequencerSystem;

namespace IOProject
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Projectile : MonoBehaviour
    {
        [SerializeReference, SubclassSelector]
        private List<ISequence> sequences;
    }
}
