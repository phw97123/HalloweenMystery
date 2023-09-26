using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
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

        public ObjectPool(List<Poolable> prefabs)
        {
            foreach (Poolable poolable in prefabs)
            {
                Queue<GameObject> queue = new Queue<GameObject>();
                for (int i = 0; i < poolable.Size; i++)
                {
                    queue.Enqueue(Instantiate(gameObject));
                }

                _pool[poolable.Tag] = queue;
                _prefabs[poolable.Tag] = poolable.Prefab;
            }
        }

        public void Push(string prefabTag, GameObject obj)
        {
            _pool[prefabTag].Enqueue(obj);
        }

        public GameObject Pop(string prefabTag)
        {
            GameObject go;
            go = _pool[prefabTag].Count > 0 ? _pool[prefabTag].Dequeue() : Instantiate(_prefabs[prefabTag]);
            return go;
        }
    }
}