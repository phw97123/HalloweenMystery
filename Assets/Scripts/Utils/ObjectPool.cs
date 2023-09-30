using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    [Serializable]
    public struct Poolable
    {
        public GameObject Prefab;
        public string Tag;
        public int Size;
    }

    public class ObjectPool : MonoBehaviour
    {
        private readonly Dictionary<string, Queue<GameObject>> _pool = new Dictionary<string, Queue<GameObject>>();
        private readonly Dictionary<string, GameObject> _prefabs = new Dictionary<string, GameObject>();
        private ResourceManager _resourceManager;

        private void Awake()
        {
            _resourceManager = ResourceManager.Instance;
        }

        public void Initialize(List<Poolable> prefabs)
        {
            foreach (Poolable poolable in prefabs)
            {
                Queue<GameObject> queue = new Queue<GameObject>();
                for (int i = 0; i < poolable.Size; i++)
                {
                    GameObject go = Instantiate(poolable.Prefab);
                    go.SetActive(false);
                    queue.Enqueue(go);
                }

                _pool.Add(poolable.Tag, queue);
                _prefabs.Add(poolable.Tag, poolable.Prefab);
            }
        }

        public void Push(string prefabTag, GameObject obj)
        {
            _pool[prefabTag].Enqueue(obj);
        }

        public GameObject Pop(string prefabTag)
        {
            GameObject go;
            if (ContainsKey(prefabTag))
            {
                go = _pool[prefabTag].Count > 0 ? _pool[prefabTag].Dequeue() : Instantiate(_prefabs[prefabTag]);
                go.SetActive(true);
            }
            else
            {
                GameObject obj = _resourceManager.LoadPrefab(prefabTag);
                _pool.Add(prefabTag, new Queue<GameObject>());
                _prefabs[prefabTag] = obj;
                go = Instantiate(obj);
            }

            return go;
        }

        public bool ContainsKey(string key)
        {
            return _pool.ContainsKey(key) && _prefabs.ContainsKey(key);
        }
    }
}