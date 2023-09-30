using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;

    [SerializeField] private Transform[] spawnPostions;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Instantiate(ResourceManager.Instance.LoadPrefab("BlueMan"));
        for (int i = 0; i < spawnPostions.Length; i++)
        {
            bool[] isAbleArray = new bool[spawnPostions.Length];
            isAbleArray[i] = true;
            WeaponManager.Singleton.CreateInteractableWeapons(isAbleArray, spawnPostions[i].position, Vector2.zero);
        }
    }

    public void ChangeScene() 
    {
        GameManager.Instance.ChangeScene(Scenes.RoomContent);
    }
}
