using System;
using System.Collections.Generic;
using UnityEngine;

namespace IOProject
{
    /// <summary>
    /// Represents a UI document that contains a collection of UI elements.
    /// </summary>
    public class HKUIDocument : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] elements;

        private readonly Dictionary<string, GameObject> elementMap = new();

        private readonly Dictionary<GameObject, Dictionary<Type, Component>> componentMap = new();

        /// <summary>
        /// Retrieves a component of type T from the UI element with the specified name.
        /// </summary>
        /// <typeparam name="T">The type of the component to retrieve.</typeparam>
        /// <param name="name">The name of the UI element.</param>
        /// <returns>The component of type T if found; otherwise, null.</returns>
        public T Q<T>(string name) where T : Component
        {
            var e = Q(name);
            if (!componentMap.TryGetValue(e, out var c))
            {
                c = new Dictionary<Type, Component>();
                componentMap[e] = c;
            }

            if (!c.TryGetValue(typeof(T), out var t))
            {
                t = e.GetComponent<T>();
                if (t == null)
                {
                    Debug.LogError($"Component not found: {typeof(T)}");
                    return null;
                }
                c[typeof(T)] = t;
            }

            return (T)t;
        }

        /// <summary>
        /// Retrieves the UI element with the specified name.
        /// </summary>
        /// <param name="name">The name of the UI element.</param>
        /// <returns>The UI element if found; otherwise, null.</returns>
        public GameObject Q(string name)
        {
            if (elementMap.Count == 0)
            {
                foreach (var element in elements)
                {
                    elementMap[element.name] = element;
                }
            }

            if (elementMap.TryGetValue(name, out var e))
            {
                return e;
            }
            else
            {
                Debug.LogError($"Element not found: {name}");
                return null;
            }
        }
    }
}