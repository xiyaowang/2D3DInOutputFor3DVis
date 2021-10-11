using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StudyMismatch2D3D.S23_Input_Android_Unity5 {

    public class ButtonsNavigManip:MonoBehaviour {

        public Button btnNavig;
        public Button btnManip;

        private Color enabledColor = new Color(0, 255, 0, 255);
        private Color disabledColor = new Color(200, 200, 200, 255);

        private void Start() {
            btnNavig.onClick.AddListener(BtnNavigOnClick);
            btnManip.onClick.AddListener(BtnManipOnClick);

            if(GlobalManager.Instance.IsModelOnManipulation)
                EnableManip();
            else
                EnableNavig();
        }

        private void BtnNavigOnClick() {
            GlobalManager.Instance.IsModelOnManipulation = false;
        }

        private void BtnManipOnClick() {
            GlobalManager.Instance.IsModelOnManipulation = true;
        }

        public void EnableNavig() {
            ColorBlock cbOfNavig = btnNavig.colors;
            cbOfNavig.normalColor = enabledColor;
            cbOfNavig.highlightedColor = enabledColor;
            btnNavig.colors = cbOfNavig;
            ColorBlock cbOfManip = btnManip.colors;
            cbOfManip.normalColor = disabledColor;
            cbOfManip.highlightedColor = disabledColor;
            btnManip.colors = cbOfManip;
        }

        public void EnableManip() {
            ColorBlock cbOfNavig = btnNavig.colors;
            cbOfNavig.normalColor = disabledColor;
            cbOfNavig.highlightedColor = disabledColor;
            btnNavig.colors = cbOfNavig;
            ColorBlock cbOfManip = btnManip.colors;
            cbOfManip.normalColor = enabledColor;
            cbOfManip.highlightedColor = enabledColor;
            btnManip.colors = cbOfManip;
        }

    }
}