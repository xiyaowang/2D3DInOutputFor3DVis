using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StudyMismatch2D3D.S23_Input_Android_Unity5 {
    public class UIManager:Singleton<UIManager> {

        public GameObject PanelOfStart;
        public GameObject PanelOfTask;
        public GameObject PanelOfFinished;

        public Text TextTask;
        public ButtonsNavigManip btnNavigManip;
        public ButtonsObjectTarget btnObjectTarget;
        public ButtonEndTraining btnFinishTraining;

        public GameObject btnMoveCamera;


        public void ChangeTaskInfo(XPTask task,InputCondition input,OutputCondition output,bool isTraining,int currentTrialIndex,int TotalIndex) {
            TextTask.text = task.ToString() + "\n"
            + input.ToString() + "\n"
            + output.ToString() + "\n"
            + (isTraining ? "Training" : "Test") + "\n"
            + (currentTrialIndex + 1) + " / " + TotalIndex;
        }

        public void ChangeObjectTarget(bool isDocking) {
            if(isDocking) {
                btnObjectTarget.ChangeTextToDocking();
            } else {
                btnObjectTarget.ChangeTextToClipping();
            }
        }
    }
}