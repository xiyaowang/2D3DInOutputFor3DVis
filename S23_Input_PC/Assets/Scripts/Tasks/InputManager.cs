using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using SpaceNavigatorDriver;

namespace StudyMismatch2D3D.S23_Input_PC {

    public class InputManager:Singleton<InputManager> {

        private GlobalManager gm;

        public static int MouseLeftButton = 0;
        public static int MouseRightButton = 1;


        public static KeyCode MouseModifier = KeyCode.LeftControl;
        public static KeyCode MouseObjectTargetModifier = KeyCode.Tab;
        public static KeyCode MouseValidate = KeyCode.Space;
        public static KeyCode MouseFinishTraining = KeyCode.LeftAlt;

        public float Gain3DMouseTransX = 0.3f;
        public float Gain3DMouseTransY = -0.2f;
        public float Gain3DMouseTransZ = 0.3f;

        public float Gain3DMouseRotX = -3f;
        public float Gain3DMouseRotY = 3f;
        public float Gain3DMouseRotZ = -3f;

        private void Start() {
            gm = GlobalManager.Instance;
        }

        private void Update() {

            if(!gm.IsTaskOn)
                return;
            switch(gm.CurrentInput) {
                case InputCondition.Tablet:
                    break;
                case InputCondition.MouseKeybaord:
                    InteractionMouseKeyboard();
                    break;
                case InputCondition.SpaceMouse:
                    InteractionSpaceMouse();
                    break;
                default:
                    throw new System.Exception("Unknown input condition.");
            }
        }


        #region MouseInteraction

        protected Vector2 previousMousePos;

        protected void InteractionMouseKeyboard() {
            if(EventSystem.current.IsPointerOverGameObject() || EventSystem.current.currentSelectedGameObject != null) {
                return;
            }

            if(Input.GetKeyDown(MouseFinishTraining) && gm.IsTraining == true) {
                gm.IsTrainingFlag = true;
                UIManager.Instance.btnFinishTraining.gameObject.SetActive(false);
            }

            if(Input.GetKeyDown(MouseObjectTargetModifier)) 
                    gm.IsObjectOnManipulation = !gm.IsObjectOnManipulation;

            if(Input.GetKeyDown(MouseValidate)) {
                gm.ValidateTrial();
            }



            if(Input.GetMouseButtonDown(MouseLeftButton) || Input.GetMouseButtonDown(MouseRightButton)) {
                previousMousePos = Input.mousePosition;
            }

            if(Input.GetMouseButton(MouseLeftButton)) {
                Vector2 currentMousePos = Input.mousePosition;
                if(Input.GetKey(MouseModifier)) {
                    float transX = (currentMousePos.x-previousMousePos.x)/Screen.width;
                    gm.Rotate(-transX * 180,Camera.main.transform.forward);
                } else {
                    //Do constrained arcball

                    //for x-translation
                    Vector3 va = GetArcballVector(new Vector2(previousMousePos.x,Screen.height/2));
                    Vector3 vb = GetArcballVector(new Vector2(currentMousePos.x,Screen.height/2));
                    float angle = Mathf.Rad2Deg*Mathf.Acos(Mathf.Min(1.0f,Vector3.Dot(va,vb)));
                    Vector3 axis = Vector3.Cross(va,vb);
                    gm.Rotate(-angle,axis);

                    //for y-translation
                    va = GetArcballVector(new Vector2(Screen.width / 2,previousMousePos.y));
                    vb = GetArcballVector(new Vector2(Screen.width / 2,currentMousePos.y));
                    angle = Mathf.Rad2Deg * Mathf.Acos(Mathf.Min(1.0f,Vector3.Dot(va,vb)));
                    axis = Vector3.Cross(va,vb);
                    gm.Rotate(-angle,axis);

                    //for both x-y
                    //Vector3 va = GetArcballVector(previousMousePos);
                    //Vector3 vb = GetArcballVector(currentMousePos);
                    //float angle = Mathf.Rad2Deg*Mathf.Acos(Mathf.Min(1.0f,Vector3.Dot(va,vb)));
                    //Vector3 axis = Vector3.Cross(va,vb);
                    //gm.Rotate(Quaternion.Inverse(Quaternion.AngleAxis(angle,axis)));
                }
                previousMousePos = currentMousePos;
            }

            if(Input.GetMouseButton(MouseRightButton)) {
                Vector2 currentMousePos = Input.mousePosition;
                if(Input.GetKey(MouseModifier)) {
                    float transY = (currentMousePos.y-previousMousePos.y)/Screen.height;
                    Vector3 translation = Camera.main.ScreenToWorldPoint(new Vector3(0,0,transY))
                        -Camera.main.ScreenToWorldPoint(new Vector3(0,0,0));
                    translation.x = 0;
                    translation.y = 0;
                    gm.Translate(translation);
                } else {
                    Vector3 translation = Camera.main.ScreenToWorldPoint(new Vector3(currentMousePos.x,currentMousePos.y,2))
                        -Camera.main.ScreenToWorldPoint(new Vector3(previousMousePos.x,previousMousePos.y,2));
                    gm.Translate(translation);
                }
                previousMousePos = currentMousePos;

            }

        }


        //https://en.wikibooks.org/wiki/OpenGL_Programming/Modern_OpenGL_Tutorial_Arcball
        protected Vector3 GetArcballVector(Vector2 pos) {
            float x = (pos.x-Screen.width/2)/(Screen.width/2);
            float y = (pos.y-Screen.height/2)/(Screen.height/2);
            float z = 0;
            //y=-y;
            float xySquare = x*x+y*y;
            if(xySquare < 1) {
                z = Mathf.Sqrt(1 - xySquare);
            } else {
                x = x / Mathf.Sqrt(xySquare);
                y = y / Mathf.Sqrt(xySquare);
            }
            return new Vector3(x,y,z);
        }

        #endregion

        protected void InteractionSpaceMouse() {
            if(Input.GetKeyDown(MouseFinishTraining) && gm.IsTraining == true) {
                gm.IsTrainingFlag = true;
                UIManager.Instance.btnFinishTraining.gameObject.SetActive(false);
            }

            if(Input.GetKeyDown(MouseObjectTargetModifier))
                gm.IsObjectOnManipulation = !gm.IsObjectOnManipulation;

            if(Input.GetKeyDown(MouseValidate)) {
                gm.ValidateTrial();
            }

            Vector3 trans = new Vector3(-SpaceNavigator.Translation.x*Gain3DMouseTransX,SpaceNavigator.Translation.y*Gain3DMouseTransY,-SpaceNavigator.Translation.z*Gain3DMouseTransZ);
            Vector3 angle = SpaceNavigator.Rotation.eulerAngles;
            if(angle.x > 180)
                angle.x = angle.x - 360;
            if(angle.y > 180)
                angle.y = angle.y - 360;
            if(angle.z > 180)
                angle.z = angle.z - 360;
            angle.x *= Gain3DMouseRotX;
            angle.y *= Gain3DMouseRotY;
            angle.z *= Gain3DMouseRotZ;
            if(gm.CurrentTask == XPTask.Docking)
                gm.Translate(trans.x,trans.z,trans.y);
            else
                gm.Translate(trans.x,trans.z,trans.y);
            if(gm.CurrentTask == XPTask.Docking)
                gm.RotateA(angle.x,angle.z,-angle.y);
            else
                gm.RotateA(angle.x,angle.z,-angle.y);
            //gm.RotateA(angle.x,angle.z,-angle.y);

            //mouse scroll wheel
            //float mouseScrollValue = Input.GetAxis("Mouse ScrollWheel");
            //if(mouseScrollValue != 0) {
            //    gm.Scale(mouseScrollValue);
            //}
        }
    }
}