#if UNITY_EDITOR
using UI;
using UnityEngine;

namespace Managers
{
    public class DemoBasicUi : UIPopup
    {
        private DemoUi _parentUi;

        private void Awake()
        {
            _parentUi = GetComponentInParent<DemoUi>();
        }

        private void Start()
        {
            _parentUi.OnStateChanged += UpdateUi;
        }

        private void UpdateUi(DemoState state)
        {
            gameObject.SetActive(state == DemoState.Basic);
        }
    }
}
#endif