using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudyMismatch2D3D.S23_Output_Screen {
    public class VolumeObject:ManipulableObject {

        protected Mesh meshX, meshY, meshZ;
        protected Matrix4x4 xInv, yInv, zInv;
        public Texture3D texture;
        protected int SamplingStep = 1;

        protected override void Start() {
            base.Start();
            xInv = Matrix4x4.TRS(Vector3.zero,Quaternion.identity,new Vector3(-1,1,1));
            yInv = Matrix4x4.TRS(Vector3.zero,Quaternion.identity,new Vector3(1,-1,1));
            zInv = Matrix4x4.TRS(Vector3.zero,Quaternion.identity,new Vector3(1,1,-1));
        }

        public override void Load() {
            base.Load();
            BuildMesh();
            material.SetTexture("_Texture3D",texture);
            isLoaded = true;

        }

        protected void BuildMesh() {
            int nbPt = texture.width;
            Vector3[] xPlaneVertex = new Vector3[nbPt*4];
            int[] xPlaneIndex = new int[nbPt*6];
            for(int i = 0; i < nbPt; i += SamplingStep) {
                xPlaneVertex[4 * i + 0] = new Vector3((float)i / nbPt - 0.5f,-0.5f,-0.5f);
                xPlaneVertex[4 * i + 1] = new Vector3((float)i / nbPt - 0.5f,-0.5f,0.5f);
                xPlaneVertex[4 * i + 2] = new Vector3((float)i / nbPt - 0.5f,0.5f,0.5f);
                xPlaneVertex[4 * i + 3] = new Vector3((float)i / nbPt - 0.5f,0.5f,-0.5f);

                xPlaneIndex[6 * i + 0] = 4 * i + 0;
                xPlaneIndex[6 * i + 1] = 4 * i + 2;
                xPlaneIndex[6 * i + 2] = 4 * i + 1;
                xPlaneIndex[6 * i + 3] = 4 * i + 0;
                xPlaneIndex[6 * i + 4] = 4 * i + 3;
                xPlaneIndex[6 * i + 5] = 4 * i + 2;
            }

            meshX = new Mesh();
            meshX.vertices = xPlaneVertex;
            meshX.triangles = xPlaneIndex;
            meshX.RecalculateBounds();
            meshX.RecalculateNormals();

            nbPt = texture.height;
            var yPlaneVertex = new Vector3[nbPt * 4];  //vertices
            var yPlaneIndex = new int[nbPt * 6];     //indices
            for(int i = 0; i < nbPt; i += SamplingStep) {
                yPlaneVertex[4 * i + 0] = new Vector3(-0.5f,(float)i / nbPt - 0.5f,-0.5f);
                yPlaneVertex[4 * i + 1] = new Vector3(-0.5f,(float)i / nbPt - 0.5f,0.5f);
                yPlaneVertex[4 * i + 2] = new Vector3(0.5f,(float)i / nbPt - 0.5f,0.5f);
                yPlaneVertex[4 * i + 3] = new Vector3(0.5f,(float)i / nbPt - 0.5f,-0.5f);
                yPlaneIndex[6 * i + 0] = 4 * i + 0;
                yPlaneIndex[6 * i + 1] = 4 * i + 2;
                yPlaneIndex[6 * i + 2] = 4 * i + 1;
                yPlaneIndex[6 * i + 3] = 4 * i + 0;
                yPlaneIndex[6 * i + 4] = 4 * i + 3;
                yPlaneIndex[6 * i + 5] = 4 * i + 2;
            }
            meshY = new Mesh();
            meshY.vertices = yPlaneVertex;
            meshY.triangles = yPlaneIndex;
            meshY.RecalculateBounds();
            meshY.RecalculateNormals();

            nbPt = texture.depth;
            var zPlaneVertex = new Vector3[nbPt * 4];  //vertices
            var zPlaneIndex = new int[nbPt * 6];     //indices
            for(int i = 0; i < nbPt; i += SamplingStep) {
                zPlaneVertex[4 * i + 0] = new Vector3(-0.5f,-0.5f,(float)i / nbPt - 0.5f);
                zPlaneVertex[4 * i + 1] = new Vector3(-0.5f,0.5f,(float)i / nbPt - 0.5f);
                zPlaneVertex[4 * i + 2] = new Vector3(0.5f,0.5f,(float)i / nbPt - 0.5f);
                zPlaneVertex[4 * i + 3] = new Vector3(0.5f,-0.5f,(float)i / nbPt - 0.5f);
                zPlaneIndex[6 * i + 0] = 4 * i + 0;
                zPlaneIndex[6 * i + 1] = 4 * i + 2;
                zPlaneIndex[6 * i + 2] = 4 * i + 1;
                zPlaneIndex[6 * i + 3] = 4 * i + 0;
                zPlaneIndex[6 * i + 4] = 4 * i + 3;
                zPlaneIndex[6 * i + 5] = 4 * i + 2;
            }
            meshZ = new Mesh();
            meshZ.vertices = zPlaneVertex;
            meshZ.triangles = zPlaneIndex;
            meshZ.RecalculateBounds();
            meshZ.RecalculateNormals();
            Scaling = new Vector3(texture.width / 300f,texture.height / 300f,texture.depth / 300f);
        }

        protected override void Render() {

            float xDot = Vector3.Dot(Rotation * Vector3.right, Camera.main.transform.forward);
            float yDot = Vector3.Dot(Rotation * Vector3.up, Camera.main.transform.forward);
            float zDot = Vector3.Dot(Rotation * Vector3.forward, Camera.main.transform.forward);

            if(Mathf.Abs(xDot) > Mathf.Abs(yDot) && Mathf.Abs(xDot) > Mathf.Abs(zDot)) {
                if(xDot < 0) {
                    //Debug.Log(1);
                    material.SetVector("_Invert",Vector3.zero);
                    material.SetMatrix("_MatInv",Matrix4x4.identity);
                } else {
                    //Debug.Log(2);
                    material.SetVector("_Invert",Vector3.right);
                    material.SetMatrix("_MatInv",xInv);
                }
                Graphics.DrawMesh(meshX,Mvm,material,gameObject.layer);
            } else if(Mathf.Abs(yDot) > Mathf.Abs(xDot) && Mathf.Abs(yDot) > Mathf.Abs(zDot)) {
                if(yDot < 0) {
                    material.SetVector("_Invert",Vector3.zero);
                    material.SetMatrix("_MatInv",Matrix4x4.identity);
                    //Debug.Log(3);

                } else {
                    //Debug.Log(4);
                    material.SetVector("_Invert",Vector3.up);
                    material.SetMatrix("_MatInv",yInv);
                }

                Graphics.DrawMesh(meshY,Mvm,material,gameObject.layer);
            } else {
                if(zDot < 0) {
                    //Debug.Log(5);
                    material.SetVector("_Invert",Vector3.zero);
                    material.SetMatrix("_MatInv",Matrix4x4.identity);
                } else {
                    //Debug.Log(6);
                    material.SetVector("_Invert",Vector3.forward);
                    material.SetMatrix("_MatInv",zInv);
                }
                Graphics.DrawMesh(meshZ,Mvm,material,gameObject.layer);
            }
        }

        public void SetClippingCenter(Vector3 cen) {
            material.SetVector("_ClipPlaneCenter",cen);

        }

        public void SetClippingNormal(Vector3 nor) {
            material.SetVector("_ClipPlaneNormal",nor);
        }

        public void SetPointRadius(float radius) {
            material.SetFloat("_PtRadius",radius);
        }

        //public void SetPoints(Vector4[] pts) {
        //    material.SetInt("_PtNumber",pts.Length);
        //    material.SetVectorArray("_PtVertex",pts);
        //}

        public void SetTrial(Vector3 c,Vector3 n) {
            material.SetVector("_RectCenter",c);
            material.SetVector("_RectNormal",n);
        }
    }
}