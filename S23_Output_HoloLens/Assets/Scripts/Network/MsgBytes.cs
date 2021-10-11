using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace StudyMismatch2D3D.S23_Output_HoloLens {

    public static class MsgBytes {
        public static byte[] Serialize(byte b) {
            return new byte[1] { b };
        }
        public static byte[] Serialize(short i) => BitConverter.GetBytes(i);
        public static byte[] Serialize(int i) => BitConverter.GetBytes(i);
        public static byte[] Serialize(float f) => BitConverter.GetBytes(f);
        public static byte[] Serialize(double d) => BitConverter.GetBytes(d);
        public static byte[] Serialize(bool b) => BitConverter.GetBytes(b);
        public static byte[] Serialize(string s) => Encoding.ASCII.GetBytes(s);
        public static byte[] Serialize(Vector2 v) {
            byte[] res = new byte[8];
            Array.Copy(Serialize(v.x),0,res,0,4);
            Array.Copy(Serialize(v.y),0,res,4,4);
            return res;
        }
        public static byte[] Serialize(Vector3 v) {
            byte[] res = new byte[12];
            Array.Copy(Serialize(v.x),0,res,0,4);
            Array.Copy(Serialize(v.y),0,res,4,4);
            Array.Copy(Serialize(v.z),0,res,8,4);
            return res;
        }
        public static byte[] Serialize(Quaternion q) {
            byte[] res = new byte[16];
            Array.Copy(Serialize(q.x),0,res,0,4);
            Array.Copy(Serialize(q.y),0,res,4,4);
            Array.Copy(Serialize(q.z),0,res,8,4);
            Array.Copy(Serialize(q.w),0,res,12,4);
            return res;
        }
        public static byte[] Serialize(List<Vector2> l) {
            byte[] res = new byte[8*l.Count];
            for(int i = 0; i < l.Count; i++) {
                Vector2 v = l[i];
                Array.Copy(Serialize(v.x),0,res,8 * i,4);
                Array.Copy(Serialize(v.y),0,res,8 * i + 4,4);
            }
            return res;
        }
        public static byte[] Serialize(List<Vector3> l) {
            byte[] res = new byte[12*l.Count];
            for(int i = 0; i < l.Count; i++) {
                Vector3 v = l[i];
                Array.Copy(Serialize(v.x),0,res,12 * i,4);
                Array.Copy(Serialize(v.y),0,res,12 * i + 4,4);
                Array.Copy(Serialize(v.z),0,res,12 * i + 8,4);
            }
            return res;
        }

        public static byte[] Serialize(Matrix4x4 m) {
            byte[] res = new byte[64];
            Array.Copy(Serialize(m.m00),0,res,0,4);
            Array.Copy(Serialize(m.m01),0,res,4,4);
            Array.Copy(Serialize(m.m02),0,res,8,4);
            Array.Copy(Serialize(m.m03),0,res,12,4);
            Array.Copy(Serialize(m.m10),0,res,16,4);
            Array.Copy(Serialize(m.m11),0,res,20,4);
            Array.Copy(Serialize(m.m12),0,res,24,4);
            Array.Copy(Serialize(m.m13),0,res,28,4);
            Array.Copy(Serialize(m.m20),0,res,32,4);
            Array.Copy(Serialize(m.m21),0,res,36,4);
            Array.Copy(Serialize(m.m22),0,res,40,4);
            Array.Copy(Serialize(m.m23),0,res,44,4);
            Array.Copy(Serialize(m.m30),0,res,48,4);
            Array.Copy(Serialize(m.m31),0,res,52,4);
            Array.Copy(Serialize(m.m32),0,res,56,4);
            Array.Copy(Serialize(m.m33),0,res,60,4);
            return res;
        }

        public static byte DeserializeByte(byte[] bytes,ref int index) {
            byte i = bytes[index];
            index += 1;
            return i;
        }

        public static short DeserializeInt16(byte[] bytes,ref int index) {
            short i = BitConverter.ToInt16(bytes,index);
            index += 2;
            return i;
        }
        public static int DeserializeInt32(byte[] bytes,ref int index) {
            int i = BitConverter.ToInt32(bytes,index);
            index += 4;
            return i;
        }
        public static float DeserializeFloat(byte[] bytes,ref int index) {
            float f = BitConverter.ToSingle(bytes,index);
            index += 4;
            return f;
        }
        public static double DeserializeDouble(byte[] bytes,ref int index) {
            double d = BitConverter.ToDouble(bytes,index);
            index += 8;
            return d;
        }
        public static bool DeserializeBool(byte[] bytes,ref int index) {
            bool b = BitConverter.ToBoolean(bytes,index);
            index += 1;
            return b;
        }
        public static string DeserializeString(byte[] bytes,ref int index) {
            int length = DeserializeInt32(bytes,ref index);
            string str = BitConverter.ToString(bytes,index);
            index += length;
            return str;
        }
        public static Vector2 DeserializeVector2(byte[] bytes,ref int index) =>
            new Vector2(DeserializeFloat(bytes,ref index),
                        DeserializeFloat(bytes,ref index));

        public static Vector3 DeserializeVector3(byte[] bytes,ref int index) =>
        new Vector3(DeserializeFloat(bytes,ref index),
                    DeserializeFloat(bytes,ref index),
                    DeserializeFloat(bytes,ref index));

        public static Quaternion DeserializeQuaternion(byte[] bytes,ref int index) =>
            new Quaternion(DeserializeFloat(bytes,ref index),
                        DeserializeFloat(bytes,ref index),
                        DeserializeFloat(bytes,ref index),
                        DeserializeFloat(bytes,ref index));

        public static Matrix4x4 DeserializeMatrix(byte[] bytes,ref int index) {
            Matrix4x4 m = new Matrix4x4();
            m.m00 = DeserializeFloat(bytes,ref index);
            m.m01 = DeserializeFloat(bytes,ref index);
            m.m02 = DeserializeFloat(bytes,ref index);
            m.m03 = DeserializeFloat(bytes,ref index);
            m.m10 = DeserializeFloat(bytes,ref index);
            m.m11 = DeserializeFloat(bytes,ref index);
            m.m12 = DeserializeFloat(bytes,ref index);
            m.m13 = DeserializeFloat(bytes,ref index);
            m.m20 = DeserializeFloat(bytes,ref index);
            m.m21 = DeserializeFloat(bytes,ref index);
            m.m22 = DeserializeFloat(bytes,ref index);
            m.m23 = DeserializeFloat(bytes,ref index);
            m.m30 = DeserializeFloat(bytes,ref index);
            m.m31 = DeserializeFloat(bytes,ref index);
            m.m32 = DeserializeFloat(bytes,ref index);
            m.m33 = DeserializeFloat(bytes,ref index);
            return m;
        }


        public static List<Vector2> DeserializeListVector2(byte[] bytes,int length,ref int index) {
            List<Vector2> list = new List<Vector2>(length);
            for(int i = 0; i < length; i++) {
                list.Add(DeserializeVector2(bytes,ref index));
            }
            return list;
        }

        public static List<Vector3> DeserializeListVector3(byte[] bytes,int length,ref int index) {
            List<Vector3> list = new List<Vector3>(length);
            for(int i = 0; i < length; i++) {
                list.Add(DeserializeVector3(bytes,ref index));
            }
            return list;
        }
    }


}