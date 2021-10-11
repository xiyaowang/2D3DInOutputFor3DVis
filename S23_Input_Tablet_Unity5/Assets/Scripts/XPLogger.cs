using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace StudyMismatch2D3D.S23_Input_Android_Unity5 {
    public static class XPLogger {
        public static bool IsFileAlreadyExisted = false;

        public static string CreateFileDocking(int userId,XPTask task,XPCondition condition,int trial) {
            IsFileAlreadyExisted = false;
            string folderPath = Application.persistentDataPath  + "/r" + userId + "/" ;
            if(!Directory.Exists(folderPath)) {
                Directory.CreateDirectory(folderPath);
            }

            string filePath = folderPath + userId+ "_" + task.ToString() + "_" + condition.ToString() + "_" + trial + ".csv";
            //string basicInfo = "User ID," + userId + "\n"
            //    + "Task," + task.ToString() + "\n"
            //    + "Condition," + condition.ToString() + "\n"
            //    + "Trial number," + trial + "\n \n"
            string basicInfo = ""
            +"Time,"
            +"Motion,"
            + "ObjectModelPosition.x,ObjectModelPosition.y,ObjectModelPosition.z,"
            + "ObjectModelRotation.x,ObjectModelRotation.y,ObjectModelRotation.z,ObjectModelRotation.w,"
            + "TargetModelPosition.x,TargetModelPosition.y,TargetModelPosition.z,"
            + "TargetModelRotation.x,TargetModelRotation.y,TargetModelRotation.z,TargetModelRotation.w,"
            + "\n";
            if(!File.Exists(filePath)) {
                File.Create(filePath).Close();
            } else {
                IsFileAlreadyExisted = true;
                return "";
            }
            File.WriteAllText(filePath,basicInfo);
            return filePath;
        }

        public static void LogDocking(string file,float t,string motion,Vector3 oPM,Quaternion oRM,Vector3 tPM,Quaternion tRM) {
            if(IsFileAlreadyExisted)
                return;
            string info = t + ","
                + motion + ","
                + oPM.x + ","+ oPM.y + ","+ oPM.z + ","
                + oRM.x  + ","+oRM.y  + ","+oRM.z  + ","+oRM.w  + ","
                + tPM.x + ","+ tPM.y + ","+ tPM.z + ","
                + tRM.x  + ","+tRM.y  + ","+tRM.z  + ","+tRM.w
                + "\n";

            File.AppendAllText(file,info);
        }

        public static string CreateFileClipping(int userId,XPTask task,XPCondition condition,int trial) {
            IsFileAlreadyExisted = false;
            string folderPath = Application.persistentDataPath  + "/r" + userId + "/" ;
            if(!Directory.Exists(folderPath)) {
                Directory.CreateDirectory(folderPath);
            }

            string filePath = folderPath + userId+ "_" + task.ToString() + "_" + condition.ToString() + "_" + trial + ".csv";
            //string basicInfo = "User ID," + userId + "\n"
            //    + "Task," + task.ToString() + "\n"
            //    + "Condition," + condition.ToString() + "\n"
            //    + "Trial number," + trial + "\n \n"
            string basicInfo = ""
            +"Time,"
            +"Motion,"
            + "PlaneCenter.x,PlaneCenter.y,PlaneCenter.z,"
            + "PlaneNormal.x,PlaneNormal.y,PlaneNormal.z,"
            + "VolumePosition.x,VolumePosition.y,VolumePosition.z,"
            + "VolumeRotation.x,VolumeRotation.y,VolumeRotation.z,VolumeRotation.w,"
            + "VolumeScaling.x,VolumeScaling.y,VolumeScaling.z,"
            + "MatrixInv"
            + "\n";

            if(!File.Exists(filePath)) {
                File.Create(filePath).Close();
            } else {
                IsFileAlreadyExisted = true;
                return "";
            }
            File.WriteAllText(filePath,basicInfo);
            return filePath;
        }

        public static void LogClipping(string file,float t,string motion,Vector3 pP,Vector3 pN,Vector3 vP,Quaternion vR,Vector3 vS,string inv) {
            if(IsFileAlreadyExisted)
                return;
            string info = t + ","
                + motion +","
                + pP.x + ","+ pP.y + ","+ pP.z + ","
                + pN.x  + ","+pN.y  + ","+pN.z + ","
                + vP.x + ","+ vP.y + ","+ vP.z + ","
                + vR.x  + ","+vR.y  + ","+vR.z  + ","+vR.w + ","
                + vS.x + ","+ vS.y + ","+ vS.z + ","
                +inv
                + "\n";

            File.AppendAllText(file,info);
        }

    }
}