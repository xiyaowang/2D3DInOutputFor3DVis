using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tango;

namespace StudyMismatch2D3D.S23_Input_Android_Unity5 {

    public class InputManager:MonoBehaviour, ITangoPose {

        //public float TranslationXAccuracy = 10e-8f;
        //public float TranslationYAccuracy = 10e-8f;
        //public float TranslationZAccuracy = 10e-8f;

        //public float GainTangoTransX = 0.001f;
        //public float GainTangoTransY = 0.001f;
        //public float GainTangoTransZ = 0.001f;

        //public float GainTangoRotX = 2f;
        //public float GainTangoRotY = 2f;
        //public float GainTangoRotZ = 2f;



        // Tango pose data for debug logging and transform update.
        [HideInInspector]
        public string m_tangoServiceVersionName = string.Empty;
        [HideInInspector]
        public float m_frameDeltaTime;
        [HideInInspector]
        public int m_frameCount;
        [HideInInspector]
        public TangoEnums.TangoPoseStatusType m_status;

        private float m_prevFrameTimestamp;

        // Tango pose data.
        private Quaternion m_tangoRotation;
        private Vector3 m_tangoPosition;

        // We use couple of matrix transformation to convert the pose from Tango coordinate
        // frame to Unity coordinate frame.
        // The full equation is:
        //     Matrix4x4 matrixuwTuc = m_matrixuwTss * matrixssTd * m_matrixdTuc;
        //
        // matrixuwTuc: Unity camera with respect to Unity world, this is the desired matrix.
        // m_matrixuwTss: Constant matrix converting start of service frame to Unity world frame.
        // matrixssTd: Device frame with repect to start of service frame, this matrix denotes the 
        //       pose transform we get from pose callback.
        // m_matrixdTuc: Constant matrix converting Unity world frame frame to device frame.
        //
        // Please see the coordinate system section online for more information:
        //     https://developers.google.com/project-tango/overview/coordinate-systems
        private Matrix4x4 m_matrixuwTss;
        private Matrix4x4 m_matrixdTuc;

        private Vector3 previousPosition;
        private Vector3 previousRotation;

        // Use this for initialization
        private void Awake() {
            // Constant matrix converting start of service frame to Unity world frame.
            m_matrixuwTss = new Matrix4x4();
            m_matrixuwTss.SetColumn(0,new Vector4(1.0f,0.0f,0.0f,0.0f));
            m_matrixuwTss.SetColumn(1,new Vector4(0.0f,0.0f,1.0f,0.0f));
            m_matrixuwTss.SetColumn(2,new Vector4(0.0f,1.0f,0.0f,0.0f));
            m_matrixuwTss.SetColumn(3,new Vector4(0.0f,0.0f,0.0f,1.0f));

            // Constant matrix converting Unity world frame frame to device frame.
            m_matrixdTuc = new Matrix4x4();
            m_matrixdTuc.SetColumn(0,new Vector4(1.0f,0.0f,0.0f,0.0f));
            m_matrixdTuc.SetColumn(1,new Vector4(0.0f,1.0f,0.0f,0.0f));
            m_matrixdTuc.SetColumn(2,new Vector4(0.0f,0.0f,-1.0f,0.0f));
            m_matrixdTuc.SetColumn(3,new Vector4(0.0f,0.0f,0.0f,1.0f));

            m_frameDeltaTime = -1.0f;
            m_prevFrameTimestamp = -1.0f;
            m_frameCount = -1;
            m_status = TangoEnums.TangoPoseStatusType.NA;
            m_tangoRotation = Quaternion.identity;
            m_tangoPosition = Vector3.zero;
        }

        private void Start() {
            m_tangoServiceVersionName = TangoApplication.GetTangoServiceVersion();

            TangoApplication tangoApplication = FindObjectOfType<TangoApplication>();
            if(tangoApplication != null) {
                tangoApplication.Register(this);
            } else {
                Debug.Log("No Tango Manager found in scene.");
            }
        }

        public void OnApplicationPause(bool pauseStatus) {
            m_frameDeltaTime = -1.0f;
            m_prevFrameTimestamp = -1.0f;
            m_frameCount = -1;
            m_status = TangoEnums.TangoPoseStatusType.NA;
            m_tangoRotation = Quaternion.identity;
            m_tangoPosition = Vector3.zero;
        }

        public void OnDestroy() {
            TangoApplication tangoApplication = FindObjectOfType<TangoApplication>();
            if(tangoApplication != null) {
                tangoApplication.Unregister(this);
            }
        }

        public void OnTangoPoseAvailable(TangoPoseData pose) {

            // Get out of here if the pose is null

            if(pose == null) {
                Debug.Log("TangoPoseDate is null.");
                return;
            }

            // The callback pose is for device with respect to start of service pose.
            if(pose.framePair.baseFrame == TangoEnums.TangoCoordinateFrameType.TANGO_COORDINATE_FRAME_START_OF_SERVICE &&
                pose.framePair.targetFrame == TangoEnums.TangoCoordinateFrameType.TANGO_COORDINATE_FRAME_DEVICE) {
                // Update the stats for the pose for the debug text
                if(pose.status_code == TangoEnums.TangoPoseStatusType.TANGO_POSE_VALID) {
                    // Create new Quaternion and Vec3 from the pose data received in the event.
                    m_tangoPosition = new Vector3((float)pose.translation[0],
                                                  (float)pose.translation[1],
                                                  (float)pose.translation[2]);

                    m_tangoRotation = new Quaternion((float)pose.orientation[0],
                                                     (float)pose.orientation[1],
                                                     (float)pose.orientation[2],
                                                     (float)pose.orientation[3]);
                    // Reset the current status frame count if the status code changed.
                    if(pose.status_code != m_status) {
                        m_frameCount = 0;
                    }

                    m_frameCount++;

                    // Compute delta frame timestamp.
                    m_frameDeltaTime = (float)pose.timestamp - m_prevFrameTimestamp;
                    m_prevFrameTimestamp = (float)pose.timestamp;

                    // Construct the start of service with respect to device matrix from the pose.
                    Matrix4x4 matrixssTd = Matrix4x4.TRS(m_tangoPosition, m_tangoRotation, Vector3.one);

                    // Converting from Tango coordinate frame to Unity coodinate frame.
                    Matrix4x4 matrixuwTuc = m_matrixuwTss * matrixssTd * m_matrixdTuc * TangoSupport.m_devicePoseRotation;

                    Vector3 translation = (Vector3)matrixuwTuc.GetColumn(3) - previousPosition;
                    previousPosition = matrixuwTuc.GetColumn(3);
                    if(GlobalManager.Instance.IsTangoOn) {
                        //translation.x = (translation.x > TranslationXAccuracy || translation.x < -TranslationXAccuracy) ? translation.x * GainTangoTransX : 0;
                        //translation.y = (translation.y > TranslationYAccuracy || translation.y < -TranslationYAccuracy) ? translation.y * GainTangoTransY : 0;
                        //translation.z = (translation.z > TranslationZAccuracy || translation.z < -TranslationZAccuracy) ? translation.z * GainTangoTransZ : 0;
                        if(GlobalManager.Instance.CurrentTask==XPTask.Docking)
                            GlobalManager.Instance.Translate(translation.x,translation.y,translation.z);
                        else
                            GlobalManager.Instance.Translate(translation.x,translation.y,translation.z);
                    }

                    Quaternion rot = Quaternion.LookRotation(matrixuwTuc.GetColumn(2),matrixuwTuc.GetColumn(1));
                    Vector3 currentRotation = rot.eulerAngles;
                    if(currentRotation.x > 180)
                        currentRotation.x = currentRotation.x - 360;
                    if(currentRotation.y > 180)
                        currentRotation.y = currentRotation.y - 360;
                    if(currentRotation.z > 180)
                        currentRotation.z = currentRotation.z - 360;
                    Vector3 rotation = currentRotation-previousRotation;
                    previousRotation = currentRotation;

                    if(GlobalManager.Instance.IsTangoOn) {
                        if(GlobalManager.Instance.CurrentTask == XPTask.Docking)
                            GlobalManager.Instance.RotateA(rotation.x,rotation.y,rotation.z);
                        else
                            GlobalManager.Instance.RotateA(rotation.x,rotation.y,rotation.z);
                    }


                    // Extract new local position
                    //transform.position = matrixuwTuc.GetColumn(3);

                    // Extract new local rotation
                    //transform.rotation = Quaternion.LookRotation(matrixuwTuc.GetColumn(2),matrixuwTuc.GetColumn(1));
                } else {
                    // if the current pose is not valid we set the pose to identity
                    m_tangoPosition = Vector3.zero;
                    m_tangoRotation = Quaternion.identity;
                }

                // Finally, apply the new pose status
                m_status = pose.status_code;
            }
        }
    }
}