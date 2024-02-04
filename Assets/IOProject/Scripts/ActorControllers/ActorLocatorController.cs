using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace IOProject
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ActorLocatorController
    {
        [Serializable]
        public class LocatorData
        {
            public string name;

            public Transform locator;
        }

        private readonly Dictionary<string, Transform> locators = new();

        public ActorLocatorController(IEnumerable<LocatorData> locators)
        {
            foreach (var locator in locators)
            {
                this.locators[locator.name] = locator.locator;
            }
        }

        public Transform GetLocator(string name)
        {
            if (locators.TryGetValue(name, out var locator))
            {
                return locator;
            }

            Assert.IsTrue(false, $"Locator with name {name} not found");
            return null;
        }
    }
}
