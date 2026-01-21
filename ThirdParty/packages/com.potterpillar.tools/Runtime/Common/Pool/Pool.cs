using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Common.Pool
{
    public struct PoolItem<T> where T : Transform
    {
        public T Item;
        public Pool Pool;

        public void Return()
        {
            if (Item == null || Pool == null) return;
            Pool.Return(Item);
        }
    }
    
    public class Pool : MonoBehaviour
    {
        [SerializeField] private Transform _poolItemPrefab;
        [SerializeField] private Transform inactiveRoot;

        // Optional: prevent double-return
        [SerializeField]
        private List<Transform> _activeItems = new();
        [SerializeField]
        private List<Transform> _inActiveItems = new();
        

        private void Awake()
        {
            EnsureRoots();
        }

        private void EnsureRoots()
        {
            if (inactiveRoot == null)
            {
                var go = new GameObject("Inactive");
                go.transform.SetParent(transform, false);
                inactiveRoot = go.transform;
            }
        }

        // Preload must always be called at least once after the creation
        public void Preload(int count)
        {
            EnsureRoots();

            if (_inActiveItems.Count >= count) return;

            for (var i = 0; i < count; i++)
            {
                var inst = Instantiate(_poolItemPrefab, inactiveRoot);
                inst.gameObject.SetActive(false);
                _inActiveItems.Add(inst.transform);
            }
        }

        public PoolItem<T> Get<T>() where T : Transform
        {
            Transform inst = null;

            // Pop until we find a valid instance (handles destroyed objects)
            while (_inActiveItems.Count > 0 && inst == null)
            {
                inst = _inActiveItems.Last();
                if (_inActiveItems.Count > 0)
                {
                    _inActiveItems.RemoveAt(_inActiveItems.Count - 1);
                }
            }

            if (inst == null)
            {
                inst = Instantiate(_poolItemPrefab, inactiveRoot).transform;
            }

            inst.gameObject.SetActive(true);
            _activeItems.Add(inst);

            PoolItem<T> item;
            item.Item = inst.GetComponent<T>();
            item.Pool = this;
            return item;
        }

        public void Return<T>(T item) where T : Transform
        {
            if (item == null) return;
            EnsureRoots();

            // Already returned? ignore
            if (!_activeItems.Contains(item)) return;

            item.gameObject.SetActive(false);
            item.transform.SetParent(inactiveRoot, false);

            _activeItems.Remove(item);
            _inActiveItems.Add(item);
        }
        
        // Add ReturnAll
        public void ReturnAll()
        {
            var list = _activeItems.ToList();
            foreach (var item in list)
            {
                Return(item);
            }
        }
    }   
}