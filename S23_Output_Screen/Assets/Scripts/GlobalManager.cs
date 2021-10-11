using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudyMismatch2D3D.S23_Output_Screen {

    public enum XPTask:byte {
        Docking = 0,
        Clipping = 1
    }

    public class GlobalManager:Singleton<GlobalManager> {

        private void Start() {
            CurrentTask = XPTask.Clipping;
            StartComm();
        }


        //GameComponent of different tasks
        public GameObject DockingGameObject;
        public GameObject ClippingGameObject;
        public DockingObject DockingObject;
        public DockingTarget DockingTarget;
        public ClippingPlaneObject ClippingPlane;
        public VolumeObject ClippingVolume;

        private XPTask currentTask = XPTask.Docking;
        public XPTask CurrentTask {
            get => currentTask;
            set {
                currentTask = value;
                if(currentTask == XPTask.Docking) {
                    if(!DockingObject.isLoaded)
                        DockingObject.Load();
                    if(!DockingTarget.isLoaded)
                        DockingTarget.Load();
                } else {
                    if(!ClippingPlane.isLoaded)
                        ClippingPlane.Load();
                    if(!ClippingVolume.isLoaded)
                        ClippingVolume.Load();
                    ClippingVolume.SetClippingCenter(ClippingPlane.Position);
                    ClippingVolume.SetClippingNormal(ClippingPlane.Rotation * Vector3.forward);
                }
            }
        }

        public bool IsTraining = true;

        public void ChangeTrial(int index) {
            if(CurrentTask == XPTask.Docking) {
                DockingGameObject.SetActive(true);
                ClippingGameObject.SetActive(false);
                DockingTrialData newData = IsTraining? DockingTrialPool.training[index] : DockingTrialPool.pool[index];
                PositionObject = newData.ObjectPosition;
                RotationObject = newData.ObjectRotation;
                PositionTarget = newData.TargetPosition;
                RotationTarget = newData.TargetRotation;
            } else {
                DockingGameObject.SetActive(false);
                ClippingGameObject.SetActive(true);
                PositionObject = new Vector3(-0.2f,0.1f,2f);
                RotationObject = Quaternion.Euler(0,90,0);
                PositionTarget = new Vector3(0,0,2f);
                RotationTarget = Quaternion.Euler(0,90,90);
                ClippingTrialData newData = IsTraining? ClippingTrialPool.training[index] : ClippingTrialPool.pool[index];
                ClippingVolume.SetTrial(newData.center,newData.normal * Vector3.forward);
                ClippingPlane.SetTrial(newData.center,newData.normal * Vector3.forward);
            }
        }

        public Vector3 PositionObject {
            get {
                return currentTask == XPTask.Docking ? DockingObject.Position : ClippingPlane.Position;
            }
            set {
                if(currentTask == XPTask.Docking) {
                    DockingObject.Position = value;
                } else {
                    ClippingPlane.Position = value;
                    ClippingVolume.SetClippingCenter(value);
                }
            }
        }

        public Vector3 PositionTarget {
            get {
                return currentTask == XPTask.Docking ? DockingTarget.Position : ClippingVolume.Position;
            }
            set {
                if(currentTask == XPTask.Docking) {
                    DockingTarget.Position = value;
                } else {
                    ClippingVolume.Position = value;
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
                    DockingTarget.Rotation = value;
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