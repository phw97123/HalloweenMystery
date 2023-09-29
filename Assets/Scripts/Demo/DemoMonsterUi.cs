#if UNITY_EDITOR


using Entites;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class DemoMonsterUI : MonoBehaviour
    {
        private DemoUi _demoUi;


        private void Awake()
        {
            _demoUi = GetComponentInParent<DemoUi>();
        }

        private void UpdateUi(DemoState state)
        {
            gameObject.SetActive(state == DemoState.CreateMonsters);
        }
    }
}

#endif