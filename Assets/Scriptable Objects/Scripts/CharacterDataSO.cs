using Components;
using Entities;
using UnityEngine;

namespace Scriptable_Objects.Scripts
{
    [CreateAssetMenu(fileName = "Character Info", menuName = "Character", order = 2)]
    public class CharacterDataSO : ScriptableObject
    {
        [Header("Character Info")] public string characterName;
        public CharacterType type;
        public CharacterStats stats;
        public bool canPlay = false;
        public GameObject imagePrefab;
    }
}