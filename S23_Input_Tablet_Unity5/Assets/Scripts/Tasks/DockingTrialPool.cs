using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudyMismatch2D3D.S23_Input_Android_Unity5 {

    public struct DockingTrialData {
        public Vector3 ObjectPosition;
        public Quaternion ObjectRotation;

        public Vector3 TargetPosition;
        public Quaternion TargetRotation;

        public DockingTrialData(Vector3 op,Quaternion or,Vector3 tp,Quaternion tr) {
            ObjectPosition = op;
            ObjectRotation = or;
            TargetPosition = tp;
            TargetRotation = tr;
        }
    }


    public static class DockingTrialPool {
        public static DockingTrialData[] training = new DockingTrialData[]{
new DockingTrialData(new Vector3(-0.3311725f,0.02421975f,2.906443f),new Quaternion(0.8916451f,0.1784625f,-0.4153394f,-0.02476328f),new Vector3(0.09593999f,-0.006651342f,2.486178f),new Quaternion(-0.2158839f,0.758921f,-0.484812f,-0.3773464f)),
new DockingTrialData(new Vector3(0.1855362f,-0.08204059f,2.978532f),new Quaternion(-0.3546533f,-0.8509668f,-0.3423687f,0.1812741f),new Vector3(-0.02003744f,-0.1158753f,2.415864f),new Quaternion(0.3390745f,-0.03434617f,-0.2323341f,0.9109718f)),
new DockingTrialData(new Vector3(0.01908538f-0.15f,0.1816384f,2.040139f),new Quaternion(-0.2494838f,0.3729008f,-0.7581578f,0.4731803f),new Vector3(0.30137f-0.15f,-0.1118539f,1.599483f),new Quaternion(-0.4136497f,0.6174394f,0.5085201f,0.4348215f)),
        };
        public static DockingTrialData[] pool = new DockingTrialData[]{
new DockingTrialData(new Vector3(0.1917408f,-0.2846918f+0.2f,2.225496f),new Quaternion(0.8440191f,-0.1061256f,-0.5214396f,0.06685742f),new Vector3(-0.1105401f,-0.1599323f+0.2f,1.722444f),new Quaternion(-0.06910634f,0.8008214f,-0.584031f,-0.1132128f)),
new DockingTrialData(new Vector3(-0.3059707f,0.2351636f,2.735566f),new Quaternion(-0.8183321f,0.01830944f,0.05044975f,-0.5722345f),new Vector3(0.04333231f,-0.1690126f,2.462383f),new Quaternion(-0.4338598f,-0.8192781f,0.1585944f,0.3397014f)),
new DockingTrialData(new Vector3(-0.1254367f,-0.2407876f+0.1f,2.076106f),new Quaternion(-0.3357966f,0.2064785f,-0.7524361f,-0.5276809f),new Vector3(0.1458422f,0.01815045f+0.1f,1.607748f),new Quaternion(-0.8996902f,-0.03162246f,-0.1072882f,0.4219558f)),
new DockingTrialData(new Vector3(-0.2653326f,-0.2419612f,2.851639f),new Quaternion(-0.2999994f,0.6370546f,-0.6247486f,0.3374181f),new Vector3(-0.2757375f,0.145301f,2.393469f),new Quaternion(-0.5734521f,0.475648f,0.6310844f,0.2159727f)),
new DockingTrialData(new Vector3(0.1499474f,-0.1044555f+0.1f,2.485618f),new Quaternion(-0.5142319f,0.6308217f,-0.08831129f,0.5743088f),new Vector3(-0.239397f,-0.2225732f+0.1f,2.044643f),new Quaternion(-0.1616347f,0.1815823f,0.9673169f,0.07211158f)),
new DockingTrialData(new Vector3(0.308969f,0.2808208f-0.1f,2.516419f),new Quaternion(0.6713168f,-0.2784128f,-0.5329514f,0.4333392f),new Vector3(-0.2028346f,0.1977146f-0.1f,2.214508f),new Quaternion(0.2051546f,0.8914665f,-0.3764873f,0.1464801f)),        };
    }
}