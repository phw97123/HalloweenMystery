#if UNITY_EDITOR


using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    //todo extends ContentListView
    public class DemoMonsterUI : MonoBehaviour
    {
        private DemoUi _demoUi;

        [SerializeField] private Button backButton;
        [SerializeField] private RectTransform content;
        [SerializeField] private GameObject buttonPrefab;

        private Dictionary<string, Object> _prefabDict = new Dictionary<string, Object>();

        private void Awake()
        {
            _demoUi = GetComponentInParent<DemoUi>();
        }

        private void Start()
        {
            backButton.onClick.AddListener(_demoUi.BackPressed);
            _demoUi.OnStateChanged += UpdateUi;

            Object[] enemies = Resources.LoadAll("Prefabs/Enemies");
            foreach (Object enemy in enemies)
            {
                _prefabDict[enemy.name] = enemy;
                GameObject go = Instantiate(buttonPrefab, content, true);
                go.name = enemy.name;
            }
        }

        private void OnDestroy()
        {
            _demoUi.OnStateChanged -= UpdateUi;
        }

        private void UpdateUi(DemoState state)
        {
            gameObject.SetActive(state == DemoState.CreateMonsters);
        }
    }
}

#endif