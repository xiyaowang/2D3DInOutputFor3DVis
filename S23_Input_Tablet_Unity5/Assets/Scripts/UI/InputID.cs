using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace StudyMismatch2D3D.S23_Input_Android_Unity5{

    [RequireComponent(typeof(InputField))]
    public class InputID:MonoBehaviour {

        private void Start() {
            InputField inputField = GetComponent<InputField>();
            inputField.onEndEdit.AddListener(delegate { InputEnd(inputField); });
            inputField.text = GlobalManager.Instance.UserIndex.ToString();
        }

        private void InputEnd(InputField userInput) {
            int id = int.Parse(userInput.text);
            GlobalManager.Instance.UserIndex = id;
        }
    }
}
