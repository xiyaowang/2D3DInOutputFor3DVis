using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace StudyMismatch2D3D.S23_Input_PC {

    [RequireComponent(typeof(Button))]
    public class ButtonFinishTraining:MonoBehaviour {
        private void Start() {
            Button btn = GetComponent<Button>();
            btn.onClick.AddListener(OnClick);
        }

        private void OnClick() {
            GlobalManager.Instance.IsTrainingFlag = true;
            this.gameObject.SetActive(false);
        }
    }
}