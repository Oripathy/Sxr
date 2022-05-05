using System;
using System.Collections.Generic;
using Model;
using UnityEngine;

namespace Factories
{
    internal class UnitFactory
    {
        private Dictionary<Type, GameObject> _prefabs;

        public UnitFactory(Dictionary<Type, GameObject> prefabs)
        {
            _prefabs = prefabs;
        }

        public T CreateModel<T>(Vector3 position, out GameObject obj) where T : BaseModel, new()
        {
            _prefabs.TryGetValue(typeof(T), out var prefab); 
            obj = GameObject.Instantiate(prefab, position, Quaternion.identity);
            return new T();
        }
    }
}