using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudyMismatch2D3D.S23_Output_HoloLens {

    public enum XPTask:byte {
        Docking = 0,
        Clipping = 1
    }

    public class GlobalManager:Singleton<GlobalManager> {

        protected Vector3 CameraPos = Vector3.zero;

        private void Start() {
            CurrentTask = XPTask.Docking;
            StartComm();
            //

        }

        private void Update() {
            //focus point
            //https://docs.microsoft.com/en-us/windows/mixed-reality/focus-point-in-unity
            var normal = -Camera.main.transform.forward;
            var position = (CurrentTask==XPTask.Docking)? PositionObject+CameraPos : PositionTarget+CameraPos;
            UnityEngine.XR.WSA.HolographicSettings.SetFocusPointForFrame(position,normal);
        }

        //GameComponent of different tasks
        public GameObject DockingGameObject;
        public GameObject ClippingGameObject;
        public DockingObject DockingObject;
        public DockingTarget DockingTarget;
        public ClippingPlaneObject ClippingPlane;
        public VolumeObject ClippingVolume;

        private XPTask currentTask= XPTask.Docking;
        public XPTask CurrentTask {
            get => currentTask;
            set {
                currentTask = value;
                if(currentTask == XPTask.Docking) {
                    if(!DockingObject.isLoaded)
                        DockingObject.Load();
                    if(!DockingTarget.isLoaded)
                        DockingTarget.Load();
                    DockingGameObject.SetActive(true);
                    ClippingGameObject.SetActive(false);
                } else {
                    if(!ClippingPlane.isLoaded)
                        ClippingPlane.Load();
                    if(!ClippingVolume.isLoaded)
                        ClippingVolume.Load();
                    DockingGameObject.SetActive(false);
                    ClippingGameObject.SetActive(true);
                    ClippingVolume.SetClippingCenter(ClippingPlane.Position);
                    ClippingVolume.SetClippingNormal(ClippingPlane.Rotation * Vector3.forward);
                }
            }
        }

        public bool IsTraining = true;

        public void ChangeTrial(int index) {
            if(CurrentTask == XPTask.Docking) {
                DockingTrialData newData = IsTraining? DockingTrialPool.training[index] : DockingTrialPool.pool[index];
                PositionObject = newData.ObjectPosition;
                RotationObject = newData.ObjectRotation;
                PositionTarget = newData.TargetPosition;
                RotationTarget = newData.TargetRotation;
            } else {
                PositionObject = new Vector3(-0.2f,0.1f,2f);
                RotationObject = Quaternion.Euler(0,90,0);
                PositionTarget = new Vector3(0,0,2f);
                RotationTarget = Quaternion.Euler(0,90,90);
                ClippingTrialData newData = IsTraining? ClippingTrialPool.training[index] : ClippingTrialPool.pool[index];
                ClippingVolume.SetTrial(newData.center,newData.normal * Vector3.forward);
                ClippingPlane.SetTrial(newData.center,newData.normal * Vector3.forward);
            }
        }

        public void MoveCameraToZero() {
            Vector3 posObj = PositionObject;
            Vector3 posTar = PositionTarget;
            CameraPos = Camera.main.transform.position;
            PositionObject = posObj;
            PositionTarget = posTar;
        }


        public Vector3 PositionObject {
            get {
                return currentTask == XPTask.Docking ? DockingObject.Position- CameraPos : ClippingPlane.Position- CameraPos;
            }
            set {
                if(currentTask == XPTask.Docking) {
                    DockingObject.Position = value+ CameraPos;
                } else {
                    ClippingPlane.Position = value + CameraPos;
                    ClippingVolume.SetClippingCenter(value + CameraPos);
                }
            }
        }

        public Vector3 PositionTarget {
            get {
                return currentTask == XPTask.Docking ? DockingTarget.Position- CameraPos : ClippingVolume.Position- CameraPos;
            }
            set {
                if(currentTask == XPTask.Docking) {
                    DockingTarget.Position = value+ CameraPos;
                } else {
                    ClippingVolume.Position = value+ CameraPos;
                }
            }
        }

        public Quaternion RotationObject {
            get {
                return currentTask == XPTask.Docking ? DockingObject.Rotation : ClippingPlane.Rotation;
            }
            set {
                if(currentTask == XPTask.Docking) {
                    DockingObject.Rotation = value;
                } else {
                    ClippingPlane.Rotation = value;
                    ClippingVolume.SetClippingNormal(value * Vector3.forward);
                }
            }
        }
        public Quaternion RotationTarget {
            get {
                return currentTask == XPTask.Docking ? DockingTarget.Rotation : ClippingVolume.Rotation;
            }
            set {
                if(currentTask == XPTask.Docking) {
                    DockingTarget.Rotation = value ;
                } else {
                    ClippingVolume.Rotation = value;
                }
            }
        }

        public Vector3 ScalingObject {
            get {
                return currentTask == XPTask.Docking ? DockingObject.Scaling : ClippingPlane.Scaling;
            }
            set {
                if(currentTask == XPTask.Docking)
                    DockingObject.Scaling = value;
                else
                    ClippingPlane.Scaling = value;
            }
        }

        public Vector3 ScalingTarget {
            get {
                return currentTask == XPTask.Docking ? DockingTarget.Scaling : ClippingVolume.Scaling;
            }
            set {
                if(currentTask == XPTask.Docking)
                    DockingTarget.Scaling = value;
                else
                    ClippingVolume.Scaling = value;
            }
        }

        public void StartComm() {
            UIManager.Instance.PanelOfWaitingInfo.SetActive(false);
            if(currentTask == XPTask.Docking) {
                DockingGameObject.SetActive(true);
                ClippingGameObject.SetActive(false);
            } else {
                DockingGameObject.SetActive(false);
                ClippingGameObject.SetActive(true);
            }
            ChangeTrial(0);
        }

        public void PauseComm() {
            UIManager.Instance.PanelOfWaitingInfo.SetActive(true);
            DockingGameObject.SetActive(false);
            ClippingGameObject.SetActive(false);
        }
    }
}