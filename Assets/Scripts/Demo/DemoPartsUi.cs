#if UNITY_EDITOR


using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;

namespace Managers
{
    public class DemoPartsUI : MonoBehaviour
    {
        private DemoUi _demoUi;

        [SerializeField] private Button backButton;
        [SerializeField] private RectTransform content;
        private Button[] _partsButtons;
        private static string[] _names;

        private void Awake()
        {
            _demoUi = GetComponentInParent<DemoUi>();
        }

        private void Start()
        {
            Object[] objects = Resources.LoadAll("Prefabs/WeaponParts");
            _names = objects.Select(obj => obj.name).ToArray();

            _demoUi.OnStateChanged += UpdateUi;
            backButton.onClick.AddListener(_demoUi.BackPressed);
            foreach (string prefabName in _names)
            {
                Object prefab = Resources.Load("Prefabs/UI/Demo/DemoButton");
                Object go = Instantiate(
                    prefab,
                    content,
                    true);
                Text text = go.GetComponentInChildren<Text>();
                text.SetText(prefabName);
                Button btn = go.GetComponentInChildren<Button>();
                btn.onClick.AddListener((() => CrateWeaponParts(prefabName)));
            }
        }

        private void CrateWeaponParts(string prefabName)
        {
            GameObject prefab = ResourceManager.Instance.LoadPrefab(prefabName);
            Instantiate(prefab, new Vector3(-1, -1), Quaternion.identity);
        }

        private void UpdateUi(DemoState state)
        {
            gameObject.SetActive(state == DemoState.CreateParts);
        }
    }
}

#endif