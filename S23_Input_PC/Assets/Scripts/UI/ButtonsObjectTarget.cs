using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StudyMismatch2D3D.S23_Input_PC {

    public class ButtonsObjectTarget:MonoBehaviour {

        public Button btnObject;
        public Button btnTarget;
        public Text textObject;
        public Text textTarget;

        private Color enabledColor = new Color(0, 255, 0, 255);
        private Color disabledColor = new Color(200, 200, 200, 255);

        private void Start() {
            btnObject.onClick.AddListener(BtnObjectOnClick);
            btnTarget.onClick.AddListener(BtnTargetOnClick);

            if(GlobalManager.Instance.IsObjectOnManipulation)
                EnableObject();
            else
                EnableTarget();
        }

        private void BtnObjectOnClick() {
            GlobalManager.Instance.IsObjectOnManipulation = true;
        }

        private void BtnTargetOnClick() {
            GlobalManager.Instance.IsObjectOnManipulation = false;
        }

        public void EnableObject() {
            ColorBlock cbOfObject = btnObject.colors;
            cbOfObject.normalColor = enabledColor;
            cbOfObject.highlightedColor = enabledColor;
            btnObject.colors = cbOfObject;
            ColorBlock cbOfTarget = btnTarget.colors;
            cbOfTarget.normalColor = disabledColor;
            cbOfTarget.highlightedColor = disabledColor;
            btnTarget.colors = cbOfTarget;
        }

        public void EnableTarget() {
            ColorBlock cbOfObject = btnObject.colors;
            cbOfObject.normalColor = disabledColor;
            cbOfObject.highlightedColor = disabledColor;
            btnObject.colors = cbOfObject;
            ColorBlock cbOfTarget = btnTarget.colors;
            cbOfTarget.normalColor = enabledColor;
            cbOfTarget.highlightedColor = enabledColor;
            btnTarget.colors = cbOfTarget;
        }

        public void ChangeTextToDocking() {
            textObject.text = "Object";
            textTarget.text = "Target";
        }
        public void ChangeTextToClipping() {
            textObject.text = "Plane";
            textTarget.text = "Volume";
        }

    }
}