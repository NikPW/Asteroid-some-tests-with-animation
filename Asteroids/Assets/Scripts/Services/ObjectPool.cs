using System;
using System.Collections.Generic;
using UnityEngine;

namespace Services {
    public class ObjectPool {

        #region Fields

        private readonly int _maxInstances;
        private readonly List<GameObject> _instances;
        
        #endregion
        #region Properties

        public int ActiveInstances {
            get => _instances.Count;
        }

        #endregion
        #region Constructors

        public ObjectPool() : this(new List<GameObject>(100)) { }
        public ObjectPool(List<GameObject> instances) {
            _instances = instances;
            _maxInstances = instances.Count;
        }

        #endregion
        #region Methods

        // Returns and deletes an object from pool
        public GameObject GetObjectFromPool(int index = 0) {
            var a = PeekObjectInPool(index);
            _instances.RemoveAt(index);
            return a;
        }
        // Returns object from pool without deleting
        public GameObject PeekObjectInPool(int index = 0) {
            if (ActiveInstances == 0) throw new Exception("Out of objects");
            return _instances[index];
        }
        // For logical difference
        public void ReturnObjectInPool(GameObject a) => InsertObjectInPool(a);
        public void InsertObjectInPool(GameObject a) {
            if (_instances.Count >= _maxInstances) throw new Exception("Pool is full");
            a.gameObject.SetActive(false);
            _instances.Add(a);
        }

        #endregion
    }
}
