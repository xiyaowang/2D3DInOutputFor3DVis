using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StudyMismatch2D3D.S23_Input_Android_Unity5{
    [RequireComponent(typeof(Dropdown))]
    public class DropdownTask:MonoBehaviour {
        private Dropdown dropdown;

        private List<string> Names = new List<string>() {
            "Docking",
            "Clipping"
        };
        private void Start() {
            dropdown = GetComponent<Dropdown>();
            dropdown.ClearOptions();
            dropdown.AddOptions(Names);
            dropdown.onValueChanged.AddListener(delegate { IndexChanged(); });
        }

        private void IndexChanged() {
            GlobalManager.Instance.CurrentTask = (XPTask)dropdown.value;
        }
    }
}