using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudyMismatch2D3D.S23_Output_HoloLens {

    public static class MsgProcessor {
        public static Dictionary<short,System.Action<byte[],int>> DicOFManip {
            get; private set;
        }

        static MsgProcessor() {
            DicOFManip = new Dictionary<short,System.Action<byte[],int>> {
                { (short)MsgKey.XPTask, LoadXPTask },

                { (short)MsgKey.IsTraining, IsTraining },

                { (short)MsgKey.IndexOfTrial, IndexOfTrial },

                { (short)MsgKey.PositionObject, PositionObject },
                { (short)MsgKey.PositionTarget, PositionTarget },
                { (short)MsgKey.RotationObject, RotationObject },

                { (short)MsgKey.RotationTarget, RotationTarget },
                { (short)MsgKey.ScalingObject, ScalingObject },
                { (short)MsgKey.ScalingTarget, ScalingTarget },
                { (short)MsgKey.StartComm, StartComm },
                { (short)MsgKey.PauseComm, PauseComm },

                { (short)MsgKey.MoveCameraToZero, MoveCameraToZero },
            };
        }

        public static void Process(byte[] data) {
            int indexOfByteArray = 0;
            short identifierOfMotion = MsgBytes.DeserializeInt16(data,ref indexOfByteArray);
            if(!DicOFManip.ContainsKey(identifierOfMotion)) {
                Debug.LogWarning("UnKnown motion identifier reveived. " + identifierOfMotion);
                return;
            }
            DicOFManip[identifierOfMotion](data,indexOfByteArray);
        }

        public static void LoadXPTask(byte[] data,int index) {
            byte i = MsgBytes.DeserializeByte(data, ref index);
            GlobalManager.Instance.CurrentTask = (XPTask)i;
        }
        public static void IsTraining(byte[] data,int index) {
            bool isTraining = MsgBytes.DeserializeBool(data, ref index);
            GlobalManager.Instance.IsTraining = isTraining;
        }

        public static void IndexOfTrial(byte[] data,int index) {
            int i = MsgBytes.DeserializeInt32(data, ref index);
            GlobalManager.Instance.ChangeTrial(i);
        }
        public static void PositionObject(byte[] data,int index) {
            Vector3 pos = MsgBytes.DeserializeVector3(data,ref index);
            GlobalManager.Instance.PositionObject = pos;
        }

        public static void PositionTarget(byte[] data,int index) {
            Vector3 pos = MsgBytes.DeserializeVector3(data,ref index);
            GlobalManager.Instance.PositionTarget = pos;
        }

        public static void RotationObject(byte[] data,int index) {
            Quaternion rot = MsgBytes.DeserializeQuaternion(data,ref index);
            GlobalManager.Instance.RotationObject = rot;
        }
        public static void RotationTarget(byte[] data,int index) {
            Quaternion rot = MsgBytes.DeserializeQuaternion(data,ref index);
            GlobalManager.Instance.RotationTarget = rot;
        }

        public static void ScalingObject(byte[] data,int index) {
            Vector3 sca = MsgBytes.DeserializeVector3(data,ref index);
            GlobalManager.Instance.ScalingObject=sca;
        }

        public static void ScalingTarget(byte[] data,int index) {
            Vector3 sca = MsgBytes.DeserializeVector3(data,ref index);
            GlobalManager.Instance.ScalingTarget = sca;
        }
        public static void StartComm(byte[] data,int index) {
            GlobalManager.Instance.StartComm();
        }

        public static void PauseComm(byte[] data,int index) {
            GlobalManager.Instance.PauseComm();
        }

        public static void MoveCameraToZero(byte[] data,int index) {
            GlobalManager.Instance.MoveCameraToZero();
        }
    }
}