using Entities;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Managers
{
    public class AttackManager : MonoBehaviour
    {
        private static AttackManager _instance = null;

        public static AttackManager Instance
        {
            get
            {
                if (_instance != null) { return _instance; }

                _instance = FindObjectOfType<AttackManager>();
                if (_instance != null) { return _instance; }

                GameObject go = new GameObject() { name = nameof(AttackManager) };
                _instance = go.AddComponent<AttackManager>();
                return _instance;
            }
        }


        private ObjectPool _pool;
        private List<Poolable> _prefabs;
        [SerializeField] private ParticleSystem defaultParticleSystem;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            _pool = gameObject.AddComponent<ObjectPool>();
            _prefabs = new List<Poolable>();
            
            //todo resourcesManager
            GameObject[] objects = Resources.LoadAll<GameObject>("Prefabs/Attacks");
            ParticleSystem ps = Resources.Load<ParticleSystem>("Prefabs/VFX/Explosion");
            defaultParticleSystem = Instantiate(ps);
            
            foreach (GameObject obj in objects)
            {
                Poolable p = new Poolable();
                p.Prefab = obj;
                p.Size = 5;
                p.Tag = obj.name;
                _prefabs.Add(p);
            }
            _pool.Initialize(_prefabs);
        }

        public void CreateProjectile(Vector2 startPosition, Vector2 direction, RangeAttackDataSO rangeAttackData)
        {
            float startDegree = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            startDegree -= (rangeAttackData.projectilesPerAttack * rangeAttackData.anglePerShot * 0.5f);

            for (int i = 0; i < rangeAttackData.projectilesPerAttack; i++)
            {
                float currentDegree = startDegree + i * rangeAttackData.anglePerShot;
                float currentRad = currentDegree * Mathf.Deg2Rad;

                Vector2 currentDirection = new Vector2(
                    x: direction.x + Mathf.Cos(currentRad),
                    y: direction.y + Mathf.Sin(currentRad)).normalized;

                GameObject go = _pool.Pop(rangeAttackData.bulletTag);
                ProjectileController controller = go.GetComponent<ProjectileController>();
                controller.Initialize(startPosition, currentDirection, currentDegree, rangeAttackData);
            }
        }

        public void InactivateGameObject(GameObject obj, string objTag, bool showFx)
        {
            if (showFx)
            {
                //todo particle system
                defaultParticleSystem.transform.position = obj.transform.position;
                defaultParticleSystem.Play();
            }

            obj.SetActive(false);
            _pool.Push(objTag, obj);
        }
    }
}