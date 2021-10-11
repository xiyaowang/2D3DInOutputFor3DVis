using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace StudyMismatch2D3D.S23_Input_Android_Unity5{

    public static class MsgCreator {

        public static byte[] XPTask(XPTask index) {
            //2 bytes for the identifier
            //1 bytes for XPTask index
            byte[] msg = new byte[3];
            Array.Copy(MsgBytes.Serialize((short)MsgKey.XPTask),0,msg,0,2);
            Array.Copy(MsgBytes.Serialize((byte)index),0,msg,2,1);
            return msg;
        }

        public static byte[] IsTraining(bool isT) {
            //2 bytes for the identifier
            //1 byte for bool
            byte[] msg = new byte[3];
            Array.Copy(MsgBytes.Serialize((short)MsgKey.IsTraining),0,msg,0,2);
            Array.Copy(MsgBytes.Serialize(isT),0,msg,2,1);
            return msg;
        }

        public static byte[] IndexOfTrial(int index) {
            //2 bytes for the identifier
            //4 bytes for int
            byte[] msg = new byte[6];
            Array.Copy(MsgBytes.Serialize((short)MsgKey.IndexOfTrial),0,msg,0,2);
            Array.Copy(MsgBytes.Serialize(index),0,msg,2,4);
            return msg;
        }

        public static byte[] PositionObject(Vector3 position) {
            //2 bytes for the identifier
            //12 bytes for the vector3 position
            byte[] msg = new byte[14];
            Array.Copy(MsgBytes.Serialize((short)MsgKey.PositionObject),0,msg,0,2);
            Array.Copy(MsgBytes.Serialize(position),0,msg,2,12);
            return msg;
        }
        public static byte[] RotationObject(Quaternion rotation) {
            //2 bytes for the identifier
            //16 bytes for the vector3 position
            byte[] msg = new byte[18];
            Array.Copy(MsgBytes.Serialize((short)MsgKey.RotationObject),0,msg,0,2);
            Array.Copy(MsgBytes.Serialize(rotation),0,msg,2,16);
            return msg;
        }
        public static byte[] ScalingObject(Vector3 scaling) {
            //2 bytes for the identifier
            //12 bytes for the vector3 position
            byte[] msg = new byte[14];
            Array.Copy(MsgBytes.Serialize((short)MsgKey.ScalingObject),0,msg,0,2);
            Array.Copy(MsgBytes.Serialize(scaling),0,msg,2,12);
            return msg;
        }
        public static byte[] PositionTarget(Vector3 position) {
            //2 bytes for the identifier
            //12 bytes for the vector3 position
            byte[] msg = new byte[14];
            Array.Copy(MsgBytes.Serialize((short)MsgKey.PositionTarget),0,msg,0,2);
            Array.Copy(MsgBytes.Serialize(position),0,msg,2,12);
            return msg;
        }
        public static byte[] RotationTarget(Quaternion rotation) {
            //2 bytes for the identifier
            //16 bytes for the vector3 position
            byte[] msg = new byte[18];
            Array.Copy(MsgBytes.Serialize((short)MsgKey.RotationTarget),0,msg,0,2);
            Array.Copy(MsgBytes.Serialize(rotation),0,msg,2,16);
            return msg;
        }
        public static byte[] ScalingTarget(Vector3 scaling) {
            //2 bytes for the identifier
            //12 bytes for the vector3 position
            byte[] msg = new byte[14];
            Array.Copy(MsgBytes.Serialize((short)MsgKey.ScalingTarget),0,msg,0,2);
            Array.Copy(MsgBytes.Serialize(scaling),0,msg,2,12);
            return msg;
        }

        public static byte[] StartComm() {
            //2 bytes for the identifier
            byte[] msg = new byte[2];
            Array.Copy(MsgBytes.Serialize((short)MsgKey.StartComm),0,msg,0,2);
            return msg;
        }
        public static byte[] PauseComm() {
            //2 bytes for the identifier
            byte[] msg = new byte[2];
            Array.Copy(MsgBytes.Serialize((short)MsgKey.PauseComm),0,msg,0,2);
            return msg;
        }

        public static byte[] MoveCameraToZero() {
            //2 bytes for the identifier
            byte[] msg = new byte[2];
            Array.Copy(MsgBytes.Serialize((short)MsgKey.MoveCameraToZero),0,msg,0,2);
            return msg;
        }
    }
}