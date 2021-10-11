using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StudyMismatch2D3D.S23_Input_PC {

    [RequireComponent(typeof(InputField))]
    public class InputIpRemoteScreen:MonoBehaviour {
        private void Start() {
            InputField inputField = GetComponent<InputField>();
            inputField.onEndEdit.AddListener(delegate { InputEnd(inputField); });
            inputField.text = UdpSetting.IpRemotePC.ToString();
        }

        private void InputEnd(InputField userInput) {
            System.Net.IPAddress newIP;
            if(System.Net.IPAddress.TryParse(userInput.text,out newIP)) {
                UdpSetting.IpRemotePC = newIP;
            }
        }
    }
}