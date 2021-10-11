using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace StudyMismatch2D3D.S23_Output_HoloLens {

    public struct ClippingTrialData {
        public Vector3 center;
        public Quaternion normal;

        public ClippingTrialData(Vector3 c,Quaternion n) {
            center = c;
            normal = n;
        }
    }

    public static class ClippingTrialPool {

        public static ClippingTrialData[] training = new ClippingTrialData[]{
                new ClippingTrialData(new Vector4(-0.01f,-0.39f,-0.38f,0),Quaternion.Euler(224f,177f,74f)),
                new ClippingTrialData(new Vector4(-0.34f,-0.28f,-0.13f,0),Quaternion.Euler(294f,287f,49f)),
                new ClippingTrialData(new Vector4(-0.32f,0.26f,-0.01f,0),Quaternion.Euler(27f,103f,195f)),
        };

        public static ClippingTrialData[] pool = new ClippingTrialData[]{
        new ClippingTrialData(new Vector4(-0.36f,-0.09f,-0.28f,0),Quaternion.Euler(136f,107f,82f)),
        new ClippingTrialData(new Vector4(0.23f,-0.06f,0.2f,0),Quaternion.Euler(273f,256f,101f)),
        new ClippingTrialData(new Vector4(0.36f,-0.3f,-0.06f,0),Quaternion.Euler(10f,124f,30f)),
        new ClippingTrialData(new Vector4(0.11f,-0.18f,-0.01f,0),Quaternion.Euler(20f,287f,50f)),
        new ClippingTrialData(new Vector4(-0.28f,-0.06f,0.14f,0),Quaternion.Euler(347f,26f,185f)),
        new ClippingTrialData(new Vector4(-0.2f,-0.08f,0.16f,0),Quaternion.Euler(17f,276f,5f)),
        new ClippingTrialData(new Vector4(-0.32f,0.25f,-0.1f,0),Quaternion.Euler(300f,288f,47f)),
        new ClippingTrialData(new Vector4(-0.03f,-0.34f,-0.04f,0),Quaternion.Euler(33f,277f,294f)),
            };
    }



}