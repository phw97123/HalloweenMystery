using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaperObjectPool : MonoBehaviour
{
    public GameObject projectilePrefab;
    public int poolSize = 10;

    private List<GameObject> objectPool; 

    private void Start()
    {
        objectPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(projectilePrefab);
            bullet.SetActive(false);
            objectPool.Add(bullet);
        }
    }

    public GameObject GetPooledProjectile()
    {
        foreach (var bullet in objectPool)
        {
            if (!bullet.activeInHierarchy)
            {
                bullet.SetActive(true);
                return bullet;
            }
        }
        return null;
    }
}
