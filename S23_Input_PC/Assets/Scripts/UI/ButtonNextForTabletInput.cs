using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StudyMismatch2D3D.S23_Input_PC {

    [RequireComponent(typeof(Button))]
    public class ButtonNextForTabletInput:MonoBehaviour {
        private void Start() {
            Button btn = GetComponent<Button>();
            btn.onClick.AddListener(OnClick);
        }

        private void OnClick() {
            UIManager.Instance.PanelOfNextCondition.SetActive(false);
            GlobalManager.Instance.IndexOfCurrentCondition += 1;
        }
    }
}