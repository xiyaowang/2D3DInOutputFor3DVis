using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace StudyMismatch2D3D.S23_Input_Android_Unity5{

    [RequireComponent(typeof(Text))]
    public class TextTango:MonoBehaviour {

        public Button ButtonOfStart;

        private void Start() {

            TangoState state;
            Text t = GetComponent<Text>();

#if UNITY_EDITOR
            state = TangoState.NotPresent;
#else
        if (!AndroidHelper.IsTangoCorePresent())
        {
            state = TangoState.NotPresent;
        }
        else if (!AndroidHelper.IsTangoCoreUpToDate())
        {
            state = TangoState.OutOfDate;
        }
        else
        {
            state = TangoState.Present;
        }
#endif
            t.text = "Tango State: " + state.ToString();
            //if(state == TangoState.Present) {
            //    ButtonOfStart.gameObject.SetActive(true);
            //} else {
            //    ButtonOfStart.gameObject.SetActive(false);
            //}
            GlobalManager.Instance.CurrentTangoState = state;
        }
    }
}