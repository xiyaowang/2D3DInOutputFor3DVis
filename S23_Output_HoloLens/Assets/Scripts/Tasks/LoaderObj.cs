using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


namespace StudyMismatch2D3D.S23_Output_HoloLens{

    public static class LoaderObj {

        public static Mesh Load(TextAsset obj, bool moveCenterToOrigin = false) {
            Mesh newMesh = new Mesh();

            string text = obj.text;
            string[] lines = text.Split('\n');

            List<int> index = new List<int>();
            //List<Vector2> Uv = new List<Vector2>();
            //List<Vector3> Normal = new List<Vector3>();

            List<Vector3> tmpVertex = new List<Vector3>();
            //List<Vector2> tmpUv = new List<Vector2>();
            //List<Vector3> tmpNormal = new List<Vector3>();

            foreach(string line in lines) {
                string[] elemets = line.Split(' ');
                switch(elemets[0]) {
                    case "#":
                        break;
                    case "v":
                        Vector3 newVertexPos = new Vector3(float.Parse(elemets[1])-0.2f,float.Parse(elemets[2])-1.6f,float.Parse(elemets[3]));
                        tmpVertex.Add(newVertexPos);
                        break;
                    case "vt":
                        //Vector2 newUv = new Vector2(float.Parse(elemets[1]),float.Parse(elemets[2]));
                        //tmpUv.Add(newUv);
                        break;
                    case "vn":
                        //Vector3 newNormal = new Vector3(float.Parse(elemets[1]),float.Parse(elemets[2]),float.Parse(elemets[3]));
                        //tmpNormal.Add(newNormal);
                        break;
                    case "f":
                        index.Add(int.Parse(elemets[1]) - 1);
                        index.Add(int.Parse(elemets[2]) - 1);
                        index.Add(int.Parse(elemets[3]) - 1);
                        break;
                    case "mtllib":
                        break;
                }
            }

            newMesh = new Mesh();
            newMesh.vertices = tmpVertex.ToArray();
            newMesh.triangles = index.ToArray();

            if(moveCenterToOrigin) {
                newMesh.RecalculateBounds();
                Vector3 center = newMesh.bounds.center;
                for(int i = 0; i < tmpVertex.Count; i++) {
                    tmpVertex[i] = tmpVertex[i] - center;
                    newMesh.vertices = tmpVertex.ToArray();
                    newMesh.RecalculateBounds();
                }
            }
            newMesh.RecalculateNormals();

            return newMesh;
        }

        public static Mesh Load(string file,bool moveCenterToOrigin = true) {
            Mesh newMesh = new Mesh();
            string filepath = Application.dataPath + "/Resources/Models/"+file+".obj";
            string text = File.ReadAllText(filepath);
            string[] lines = text.Split('\n');

            List<int> index = new List<int>();
            //List<Vector2> Uv = new List<Vector2>();
            //List<Vector3> Normal = new List<Vector3>();

            List<Vector3> tmpVertex = new List<Vector3>();
            //List<Vector2> tmpUv = new List<Vector2>();
            //List<Vector3> tmpNormal = new List<Vector3>();

            foreach(string line in lines) {
                string[] elemets = line.Split(' ');
                switch(elemets[0]) {
                    case "#":
                        break;
                    case "v":
                        Vector3 newVertexPos = new Vector3(float.Parse(elemets[1]),float.Parse(elemets[2]),float.Parse(elemets[3]));
                        tmpVertex.Add(newVertexPos);
                        break;
                    case "vt":
                        //Vector2 newUv = new Vector2(float.Parse(elemets[1]),float.Parse(elemets[2]));
                        //tmpUv.Add(newUv);
                        break;
                    case "vn":
                        //Vector3 newNormal = new Vector3(float.Parse(elemets[1]),float.Parse(elemets[2]),float.Parse(elemets[3]));
                        //tmpNormal.Add(newNormal);
                        break;
                    case "f":
                        index.Add(int.Parse(elemets[1]) - 1);
                        index.Add(int.Parse(elemets[2]) - 1);
                        index.Add(int.Parse(elemets[3]) - 1);
                        break;
                    case "mtllib":
                        break;
                }
            }

            newMesh = new Mesh();
            newMesh.vertices = tmpVertex.ToArray();
            newMesh.triangles = index.ToArray();
            newMesh.RecalculateBounds();

            if(moveCenterToOrigin) {
                Vector3 center = newMesh.bounds.center;
                for(int i = 0; i < tmpVertex.Count; i++) {
                    tmpVertex[i] = tmpVertex[i] - center;
                    newMesh.vertices = tmpVertex.ToArray();
                    newMesh.RecalculateBounds();
                }
            }
            newMesh.RecalculateNormals();

            return newMesh;
        }
    }
}