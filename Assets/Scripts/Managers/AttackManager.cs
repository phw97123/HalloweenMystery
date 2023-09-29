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
            
            //todo resourcesManager and only required prefabs
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

        public void MeleeAttack(
            string prefabTag,
            Vector2 startPosition,
            Vector2 direction,
            int currentAttackCount,
            float degree,
            MeleeAttackDataSO attackData)
        {
            GameObject go = _pool.Pop(prefabTag);
            AttackController controller = go.GetComponent<AttackController>();

            //todo apply critical
            controller.Initialize(
                startPosition: startPosition,
                direction: direction,
                degree: degree,
                currentAttackCount: currentAttackCount,
                isCritical: false,
                attackData: attackData,
                attackTag : prefabTag);
        }

        public void RangeAttack(Vector2 startPosition, Vector2 direction, RangeAttackDataSO rangeAttackData)
        {
            //todo migrate to range attack
            float startDegree = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            startDegree -= (rangeAttackData.projectilesPerAttack * rangeAttackData.anglePerShot * 0.5f);

            for (int i = 0; i < rangeAttackData.projectilesPerAttack; i++)
            {
                float currentDegree = startDegree + i * rangeAttackData.anglePerShot;
                float currentRad = currentDegree * Mathf.Deg2Rad;

                Vector2 currentDirection = new Vector2(
                    x: direction.x + Mathf.Cos(currentRad),
                    y: direction.y + Mathf.Sin(currentRad)).normalized;

                //todo only exists it 
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
            }

            obj.SetActive(false);
            _pool.Push(objTag, obj);
        }
    }
}