using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;


namespace StudyMismatch2D3D.S23_Input_PC {

    public class GlobalManager:Singleton<GlobalManager> {

        //Other Game Managers
        private UdpManager udp;
        private UIManager ui;

        protected override void Awake() {
            base.Awake();
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 15;
        }

        private void Start() {
            udp = UdpManager.Instance;
            ui = UIManager.Instance;
        }

        public bool IsSyncOn = true;

        //GameComponent of different tasks
        public GameObject DockingGameObject;
        public GameObject ClippingGameObject;
        public DockingObject DockingObject;
        public DockingTarget DockingTarget;
        public ClippingPlaneObject ClippingPlane;
        public VolumeObject ClippingVolume;

        //Elements for recording
        private string currentFileToLog;
        private float timeOfStartTrial;

        public int UserIndex = 2;

        private XPCondition[] arrayOfXPConditions;
        private InputCondition[] arrayOfInput;
        private OutputCondition[] arrayOfOutput;

        public XPTask FirstTask = XPTask.Clipping;

        private XPTask currentTask;
        public XPTask CurrentTask {
            get => currentTask;
            set {
                if(IsSyncOn) {
                    udp.RecreateSocket(UdpSetting.IpToListen,UdpSetting.PortToListen,UdpSetting.IpRemotePC,UdpSetting.PortToSend);
                    udp.SendUDPMessage(MsgCreator.XPTask(value));
                    udp.SendUDPMessage(MsgCreator.PauseComm());
                    udp.RecreateSocket(UdpSetting.IpToListen,UdpSetting.PortToListen,UdpSetting.IpHoloLens,UdpSetting.PortToSend);
                    udp.SendUDPMessage(MsgCreator.XPTask(value));
                    udp.SendUDPMessage(MsgCreator.PauseComm());
                }
                currentTask = value;
                if(currentTask == XPTask.Docking) {
                    DockingObject.Load();
                    DockingTarget.Load();
                    DockingGameObject.SetActive(true);
                    ClippingGameObject.SetActive(false);
                    ui.ChangeObjectTarget(true);
                } else {
                    ClippingPlane.Load();
                    ClippingVolume.Load();
                    ClippingVolume.SetClippingCenter(ClippingPlane.Position);
                    ClippingVolume.SetClippingNormal(ClippingPlane.Rotation * Vector3.forward);
                    DockingGameObject.SetActive(false);
                    ClippingGameObject.SetActive(true);
                    ui.ChangeObjectTarget(false);
                }
                IsTraining = true;
            }
        }

        private int indexOfCurrentCondition = 0;
        public int IndexOfCurrentCondition {
            get => indexOfCurrentCondition;
            set {
                if(value >= 6) {
                    if(CurrentTask == XPTask.Docking) {
                        CurrentTask = XPTask.Clipping;
                        indexOfCurrentCondition = 0;
                    } else {
                        ui.PanelOfTask.SetActive(false);
                        ui.PanelOfFinished.SetActive(true);
                        indexOfCurrentCondition = value;
                        return;
                    }
                } else {
                    indexOfCurrentCondition = value;
                }
                IsTraining = true;
                CurrentInput = arrayOfInput[indexOfCurrentCondition];
                CurrentOutput = arrayOfOutput[indexOfCurrentCondition];

            }
        }

        private InputCondition currentInput = InputCondition.MouseKeybaord;
        public InputCondition CurrentInput {
            get => currentInput;
            set {
                if(currentInput == InputCondition.Tablet && value != InputCondition.Tablet) {
                    ui.PanelOfTask.SetActive(true);
                    ui.PanelOfNextCondition.SetActive(false);
                }
                currentInput = value;
                if(currentInput == InputCondition.Tablet) {
                    ui.PanelOfTask.SetActive(false);
                    ui.PanelOfNextCondition.SetActive(true);
                } else {
                    ui.ChangeTaskInfo(CurrentTask,CurrentInput,CurrentOutput,IsTraining,IndexOfCurrentTrial,IsTraining ? arrayOfTrainingIndex.Length : arrayOfTrialIndex.Length);
                }
            }
        }

        private OutputCondition currentOutput = OutputCondition.Screen;
        public OutputCondition CurrentOutput {
            get => currentOutput;
            set {
                if(currentOutput == OutputCondition.Screen && value==OutputCondition.HoloLens) {
                    udp.SendUDPMessage(MsgCreator.PauseComm());
                    udp.RecreateSocket(UdpSetting.IpToListen,UdpSetting.PortToListen,UdpSetting.IpHoloLens,UdpSetting.PortToSend);
                    udp.SendUDPMessage(MsgCreator.StartComm());
                } else if(currentOutput == OutputCondition.HoloLens && value == OutputCondition.Screen) {
                    udp.SendUDPMessage(MsgCreator.PauseComm());
                    udp.RecreateSocket(UdpSetting.IpToListen,UdpSetting.PortToListen,UdpSetting.IpRemotePC,UdpSetting.PortToSend);
                    udp.SendUDPMessage(MsgCreator.StartComm());
                }
                currentOutput = value;
                ui.ChangeTaskInfo(CurrentTask,CurrentInput,CurrentOutput,IsTraining,IndexOfCurrentTrial,IsTraining ? arrayOfTrainingIndex.Length : arrayOfTrialIndex.Length);
                if(currentOutput == OutputCondition.Screen)
                    ui.btnMoveCamera.SetActive(false);
                else
                    ui.btnMoveCamera.SetActive(true);
            }
        }

        public bool IsTrainingFlag = false;

        private bool isTraining = true;
        public bool IsTraining {
            get => isTraining;
            set {
                if(IsSyncOn)
                    udp.SendUDPMessage(MsgCreator.IsTraining(value));
                isTraining = value;
                IndexOfCurrentTrial = 0;
                ui.btnFinishTraining.gameObject.SetActive(value);
                ui.ChangeTaskInfo(CurrentTask,CurrentInput,CurrentOutput,IsTraining,IndexOfCurrentTrial,IsTraining ? arrayOfTrainingIndex.Length : arrayOfTrialIndex.Length);
            }
        }

        private int[] arrayOfTrainingIndex = new int[]{0,1,2};
        private int[] arrayOfTrialIndex = new int[]{0,1,2,3,4,5};

        private int numberOfCurrentTrial = 0;

        private int indexOfCurrentTrial = 0;
        public int IndexOfCurrentTrial {
            get => indexOfCurrentTrial;
            set {
                indexOfCurrentTrial = value;
                numberOfCurrentTrial = isTraining ? arrayOfTrainingIndex[value]:arrayOfTrialIndex[value];
                ChangeTrial(numberOfCurrentTrial);
                if(IsSyncOn) {
                    udp.SendUDPMessage(MsgCreator.XPTask(CurrentTask));
                    udp.SendUDPMessage(MsgCreator.IsTraining(IsTraining));
                    udp.SendUDPMessage(MsgCreator.IndexOfTrial(numberOfCurrentTrial));
                }
                ui.ChangeTaskInfo(CurrentTask,CurrentInput,CurrentOutput,IsTraining,IndexOfCurrentTrial,IsTraining ? arrayOfTrainingIndex.Length : arrayOfTrialIndex.Length);
                IsModelOnManipulation = true;
                IsObjectOnManipulation = true;
            }
        }

        public void ChangeTrial(int index) {

            timeOfStartTrial = Time.time;
            if(!IsTraining) {
                if(CurrentTask == XPTask.Docking)
                    currentFileToLog = XPLogger.CreateFileDocking(UserIndex,CurrentTask,arrayOfXPConditions[indexOfCurrentCondition],numberOfCurrentTrial);
                else
                    currentFileToLog = XPLogger.CreateFileClipping(UserIndex,CurrentTask,arrayOfXPConditions[indexOfCurrentCondition],numberOfCurrentTrial);
            }

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

        public void ResetTrial() {
            if(CurrentTask == XPTask.Docking) {
                DockingTrialData newData = isTraining? DockingTrialPool.training[numberOfCurrentTrial] : DockingTrialPool.pool[numberOfCurrentTrial];
                PositionObject = newData.ObjectPosition;
                RotationObject = newData.ObjectRotation;
                PositionTarget = newData.TargetPosition;
                RotationTarget = newData.TargetRotation;
                if(!IsTraining)
                    XPLogger.LogDocking(currentFileToLog,Time.time - timeOfStartTrial,"Reset",PositionObject,RotationObject,PositionTarget,RotationTarget);

            } else {
                PositionObject = new Vector3(-0.2f,0.1f,2f);
                RotationObject = Quaternion.Euler(0,90,0);
                PositionTarget = new Vector3(0,0,2f);
                RotationTarget = Quaternion.Euler(0,90,90);
                //ClippingTrialData newData = isTraining? ClippingTrialPool.training[numberOfCurrentTrial] : ClippingTrialPool.pool[numberOfCurrentTrial];
                //ClippingVolume.SetTrial(newData.center,newData.normal * Vector3.forward);
                //ClippingPlane.SetTrial(newData.center,newData.normal * Vector3.forward);
                if(!IsTraining)
                    XPLogger.LogClipping(currentFileToLog,Time.time - timeOfStartTrial,"Reset",PositionObject,RotationObject * Vector3.forward,PositionTarget,RotationTarget,ScalingTarget,ClippingVolume.CurrentMatrixInv);
            }
        }

        public void ValidateTrial() {
            if((Time.time - timeOfStartTrial) < 3)
                return;
            if(IsTrainingFlag) {
                IsTraining = false;
                IsTrainingFlag = false;
                return;
            }
            if(isTraining) {
                IndexOfCurrentTrial = (IndexOfCurrentTrial >= (arrayOfTrainingIndex.Length - 1)) ? 0 : IndexOfCurrentTrial + 1;
            } else {
                if(IndexOfCurrentTrial < (arrayOfTrialIndex.Length - 1)) {
                    IndexOfCurrentTrial += 1;
                } else {
                    IndexOfCurrentCondition += 1;
                }
            }
        }

        private bool isModelOnManipulation = true;
        public bool IsModelOnManipulation {
            get => isModelOnManipulation;
            set {
                isModelOnManipulation = value;
                if(isModelOnManipulation)
                    ui.btnNavigManip.EnableManip();
                else
                    ui.btnNavigManip.EnableNavig();
            }
        }

        private bool isObjectOnManipulation = true;
        public bool IsObjectOnManipulation {
            get => isObjectOnManipulation;
            set {
                isObjectOnManipulation = value;
                if(isObjectOnManipulation)
                    ui.btnObjectTarget.EnableObject();
                else
                    ui.btnObjectTarget.EnableTarget();
            }
        }

        public bool IsTaskOn = false;
        public void StartXP() {

            int i = UserIndex%12;            
            arrayOfXPConditions = ExperimentConditions.XPConditions[i];
            arrayOfInput = ExperimentConditions.InputOrders[i];
            arrayOfOutput = ExperimentConditions.OutputOrders[i];
            arrayOfTrialIndex = ExperimentConditions.TaskOrders[UserIndex % 12];

            CurrentTask = FirstTask;

            if(arrayOfOutput[0] == OutputCondition.Screen) {
                udp.RecreateSocket(UdpSetting.IpToListen,UdpSetting.PortToListen,UdpSetting.IpRemotePC,UdpSetting.PortToSend);
            } else {
                udp.RecreateSocket(UdpSetting.IpToListen,UdpSetting.PortToListen,UdpSetting.IpHoloLens,UdpSetting.PortToSend);
            }
            udp.SendUDPMessage(MsgCreator.StartComm());
            IndexOfCurrentCondition = 0;
            IsTaskOn = true;

        }

        #region Transformation

        public static bool isTranslationXEnbaled = true;
        public static bool isTranslationYEnbaled = true;
        public static bool isTranslationZEnbaled = true;
        public static bool isRotationXEnbaled = true;
        public static bool isRotationYEnbaled = true;
        public static bool isRotationZEnbaled = true;
        public static bool isScalingXEnbaled = true;
        public static bool isScalingYEnbaled = true;
        public static bool isScalingZEnbaled = true;

        public Vector3 PositionObject {
            get {
                return currentTask == XPTask.Docking ? DockingObject.Position : ClippingPlane.Position;
            }
            set {
                if(IsSyncOn)
                    udp.SendUDPMessage(MsgCreator.PositionObject(value));
                if(currentTask == XPTask.Docking) {
                    DockingObject.Position = value;
                    if(!IsTraining)
                        XPLogger.LogDocking(currentFileToLog,Time.time - timeOfStartTrial,"PositionObject",PositionObject,RotationObject,PositionTarget,RotationTarget);
                } else {
                    ClippingPlane.Position = value;
                    ClippingVolume.SetClippingCenter(value);
                    if(!IsTraining)
                        XPLogger.LogClipping(currentFileToLog,Time.time - timeOfStartTrial,"PositionPlane",PositionObject,RotationObject * Vector3.forward,PositionTarget,RotationTarget,ScalingTarget,ClippingVolume.CurrentMatrixInv);
                }
            }
        }

        public Vector3 PositionTarget {
            get {
                return currentTask == XPTask.Docking ? DockingTarget.Position : ClippingVolume.Position;
            }
            set {
                if(IsSyncOn)
                    udp.SendUDPMessage(MsgCreator.PositionTarget(value));
                if(currentTask == XPTask.Docking) {
                    DockingTarget.Position = value;
                    if(!IsTraining)
                        XPLogger.LogDocking(currentFileToLog,Time.time - timeOfStartTrial,"PositionTarget",PositionObject,RotationObject,PositionTarget,RotationTarget);
                } else {
                    ClippingVolume.Position = value;
                    if(!IsTraining)
                        XPLogger.LogClipping(currentFileToLog,Time.time - timeOfStartTrial,"PositionVolume",PositionObject,RotationObject * Vector3.forward,PositionTarget,RotationTarget,ScalingTarget,ClippingVolume.CurrentMatrixInv);
                }
            }
        }

        public Quaternion RotationObject {
            get {
                return currentTask == XPTask.Docking ? DockingObject.Rotation : ClippingPlane.Rotation;
            }
            set {
                if(IsSyncOn)
                    udp.SendUDPMessage(MsgCreator.RotationObject(value));
                if(currentTask == XPTask.Docking) {
                    DockingObject.Rotation = value;
                    if(!IsTraining)
                        XPLogger.LogDocking(currentFileToLog,Time.time - timeOfStartTrial,"RotationObject",PositionObject,RotationObject,PositionTarget,RotationTarget);
                } else {
                    ClippingPlane.Rotation = value;
                    ClippingVolume.SetClippingNormal(value * Vector3.forward);
                    if(!IsTraining)
                        XPLogger.LogClipping(currentFileToLog,Time.time - timeOfStartTrial,"RotationPlane",PositionObject,RotationObject * Vector3.forward,PositionTarget,RotationTarget,ScalingTarget,ClippingVolume.CurrentMatrixInv);
                }
            }
        }
        public Quaternion RotationTarget {
            get {
                return currentTask == XPTask.Docking ? DockingTarget.Rotation : ClippingVolume.Rotation;
            }
            set {
                if(IsSyncOn)
                    udp.SendUDPMessage(MsgCreator.RotationTarget(value));
                if(currentTask == XPTask.Docking) {
                    DockingTarget.Rotation = value;
                    if(!IsTraining)
                        XPLogger.LogDocking(currentFileToLog,Time.time - timeOfStartTrial,"RotationTarget",PositionObject,RotationObject,PositionTarget,RotationTarget);
                } else {
                    ClippingVolume.Rotation = value;
                    if(!IsTraining)
                        XPLogger.LogClipping(currentFileToLog,Time.time - timeOfStartTrial,"RotationVolume",PositionObject,RotationObject * Vector3.forward,PositionTarget,RotationTarget,ScalingTarget,ClippingVolume.CurrentMatrixInv);
                }
            }
        }

        public Vector3 ScalingObject {
            get {
                return currentTask == XPTask.Docking ? DockingObject.Scaling : ClippingPlane.Scaling;
            }
            set {
                if(IsSyncOn)
                    udp.SendUDPMessage(MsgCreator.ScalingObject(value));
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
                if(IsSyncOn)
                    udp.SendUDPMessage(MsgCreator.ScalingTarget(value));
                if(currentTask == XPTask.Docking)
                    DockingTarget.Scaling = value;
                else
                    ClippingVolume.Scaling = value;
            }
        }

        public void Translate(Vector3 trans) {
            Translate(trans.x,trans.y,trans.z);
        }

        public void Translate(float x,float y,float z) {
            x = isTranslationXEnbaled ? x : 0;
            y = isTranslationYEnbaled ? y : 0;
            z = isTranslationZEnbaled ? z : 0;
            if(x == 0 && y == 0 && z == 0)
                return;
            Vector3 distance = new Vector3(x,y,z);
            if(IsModelOnManipulation) {
                if(IsObjectOnManipulation) {
                    if(CurrentTask == XPTask.Docking) {
                        PositionObject += distance;
                    } else {
                        PositionObject += distance;
                        //Vector3 planeNormal = RotationObject*Vector3.forward;
                        //float d = distance.x*Vector3.Dot(planeNormal,Vector3.right)/planeNormal.magnitude
                        //    +distance.y*Vector3.Dot(planeNormal,Vector3.up) / planeNormal.magnitude
                        //    +distance.z*Vector3.Dot(planeNormal,Vector3.forward) / planeNormal.magnitude;
                        //PositionObject += d * planeNormal;
                        //if(Mathf.Abs(y) >= Mathf.Abs(x) && Mathf.Abs(y) >= Mathf.Abs(z))
                        //    PositionObject += y * planeNormal;
                    }
                } else {
                    PositionTarget += distance;
                    PositionObject += distance;

                }
            } else {
                PositionObject -= distance;
                PositionTarget -= distance;
            }
        }

        public void Rotate(float Tx,float Ty,float Tz) {
            float x = Ty*3.14f;
            float y = -Tx*3.14f;
            float z = Tz*3.14f;

            x = isRotationXEnbaled ? x : 0;
            y = isRotationYEnbaled ? y : 0;
            z = isRotationZEnbaled ? z : 0;
            if(x == 0 && y == 0 && z == 0)
                return;
            if(IsModelOnManipulation) {
                if(IsObjectOnManipulation) {
                    if(CurrentTask == XPTask.Docking) { 
                        Quaternion newRotation = Quaternion.Euler(x,y,z)*RotationObject;
                        newRotation.Normalize();
                        RotationObject = newRotation;
                    } else {
                        //Vector3 xPrime = RotationObject*Vector3.right; 
                        //Vector3 yPrime = RotationObject*Vector3.up;
                        //Vector3 zPrime = RotationObject*Vector3.forward;
                        //float x2 = Ty * Vector3.Dot(xPrime,Vector3.right)/xPrime.magnitude+ Tx * Vector3.Dot(xPrime,Vector3.up)/xPrime.magnitude+Tz * Vector3.Dot(xPrime,Vector3.forward)/xPrime.magnitude;
                        //float y2 = Ty * Vector3.Dot(yPrime,Vector3.right)/yPrime.magnitude+ Tx * Vector3.Dot(yPrime,Vector3.up)/yPrime.magnitude+Tz * Vector3.Dot(yPrime,Vector3.forward)/yPrime.magnitude;
                        //float z2 = Tx * Vector3.Dot(xPrime,Vector3.right)/xPrime.magnitude+ Ty * Vector3.Dot(xPrime,Vector3.up)/xPrime.magnitude+Tz * Vector3.Dot(xPrime,Vector3.forward)/xPrime.magnitude;
                        //Quaternion newRotation =Quaternion.AngleAxis(-y2,yPrime)* RotationObject;
                        //newRotation.Normalize();
                        //newRotation = Quaternion.AngleAxis(x2,xPrime) * newRotation;
                        //newRotation.Normalize();
                        //RotationObject = newRotation;
                        Quaternion newRotation = Quaternion.Euler(x,y,z)*RotationObject;
                        newRotation.Normalize();
                        RotationObject = newRotation;
                    }
                } else {
                    Quaternion newRotation = Quaternion.Euler(x,y,z)*RotationTarget;
                    newRotation.Normalize();
                    RotationTarget = newRotation;

                    //Now deal with the object
                    PositionObject = ProcessRotatePositionAroundAPoint(PositionObject,PositionTarget,x,y,z);
                    RotationObject = ProcessRotateRotationAroundAPoint(RotationObject,-x,-y,-z);
                }
            } else {
                PositionObject = ProcessRotateViewPosition(PositionObject,x,y,z);
                PositionTarget = ProcessRotateViewPosition(PositionTarget,x,y,z);
                RotationObject = ProcessRotateViewRotation(RotationObject,x,y,z);
                RotationTarget = ProcessRotateViewRotation(RotationTarget,x,y,z);
            }
        }

        public void RotateA(float x,float y,float z) {
            float Tx = -y/3.14f;
            float Ty = x/3.14f;
            float Tz = z/3.14f;
            Rotate(Tx,Ty,Tz);
        }

        public void Rotate(float angle, Vector3 axis) {
            Vector3 a = Quaternion.AngleAxis(angle,axis).eulerAngles;
            a.x = (a.x > 180) ? a.x -360 : a.x;
            a.y = (a.y > 180) ? a.y - 360 : a.y;
            a.z = (a.z > 180) ? a.z - 360 : a.z;

            RotateA(a.x,a.y,a.z);
        }

        private Vector3 ProcessRotatePositionAroundAPoint(Vector3 posToRoatate, Vector3 rotationCenter, float x, float y, float z) {
            float distanceToCenter = Vector3.Distance(posToRoatate,rotationCenter);
            Vector3 directionFromCenter = (posToRoatate-rotationCenter).normalized;
            Vector3 newDirectionFromCenter = (Quaternion.Euler(x,y,z) * directionFromCenter).normalized;
            return distanceToCenter * newDirectionFromCenter + rotationCenter;
        }

        private Quaternion ProcessRotateRotationAroundAPoint(Quaternion intialRot,float x,float y,float z) {
            return (Quaternion.Euler(-x,-y,-z) * intialRot).normalized;
        }

        private Vector3 ProcessRotateViewPosition(Vector3 initialPos,float x,float y,float z) {
            float distanceToCamera = Vector3.Distance(initialPos,Vector3.zero); //Camera is always at 0
            Vector3 directionFromCamera = (initialPos - Vector3.zero).normalized;
            Vector3 newDirectionFromCamera = (Quaternion.Euler(-x,-y,-z) * directionFromCamera).normalized;
            return distanceToCamera * newDirectionFromCamera;
        }

        private Quaternion ProcessRotateViewRotation(Quaternion intialRot,float x,float y,float z) {
            return (Quaternion.Euler(-x,-y,-z) * intialRot).normalized;
        }

        public void Scale(float scaling) {
            float x = isScalingXEnbaled ? scaling : 0;
            float y = isScalingYEnbaled ? scaling : 0;
            float z = isScalingZEnbaled ? scaling : 0;
            if(x == 1 & y == 1 & z == 1)
                return;
            Vector3 oldScale = ScalingObject;
            oldScale.x *= 1+x;
            oldScale.y *= 1+y;
            oldScale.z *= 1+z;
            ScalingObject = oldScale;
            oldScale = ScalingTarget;
            oldScale.x *= 1+x;
            oldScale.y *= 1+y;
            oldScale.z *= 1+z;
            ScalingTarget = oldScale;
        }


        #endregion


    }
}