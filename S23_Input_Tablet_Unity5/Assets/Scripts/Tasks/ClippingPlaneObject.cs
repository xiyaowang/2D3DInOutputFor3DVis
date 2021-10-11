using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudyMismatch2D3D.S23_Input_Android_Unity5 {
    public class ClippingPlaneObject:ManipulableObject {

        public VolumeObject volume;
        protected Vector3 c;
        protected Vector3 n;
        public void SetTrial(Vector3 cc,Vector3 nn) {
            c = cc;
            n = nn;
        }

        protected Mesh meshAxis;
        protected Mesh meshFrame;
        protected Mesh meshPivot;

        public Shader shaderForAxis;
        protected Material materialForAxis;
        protected Matrix4x4 MvmOfScreenAxis;
        protected Matrix4x4 MvmOfPivot;

        protected Material materialForPivot;
        public void SetPivotColor(Color cc) {
            materialForPivot.SetColor("_Color",cc);
        }

        public override void ComputeMvm() {
            base.ComputeMvm();
            MvmOfScreenAxis = Matrix4x4.TRS(position,Quaternion.identity,Vector3.one);
            MvmOfPivot = Matrix4x4.TRS(position,Quaternion.identity,Vector3.one * 0.01f);

            if(isLoaded) {
                Matrix4x4 m = volume.Mvm;
                Vector3 rc = m.MultiplyPoint(c);
                Vector3 rn = (volume.Rotation*n);
                float dist = Mathf.Abs(distanceToPlane(rc,rn,position));
                if(dist < 0.015f) {
                    Vector3 p = m.inverse.MultiplyPoint(position);
                    if(p.x >= -0.52 && p.x <= 0.52 && p.y >= -0.52 && p.y <= 0.52 && p.z >= -0.52 && p.z <= 0.52)
                        SetPivotColor(Color.red);
                } else {
                    SetPivotColor(Color.white);
                }
            }


        }

        protected override void Awake() {
            scaling = new Vector3(0.8f,0.8f,0.002f);
            base.Awake();
            materialForAxis = new Material(shaderForAxis);
            materialForPivot = new Material(shader);

        }

        public override void Load() {
            base.Load();
            mesh = BuildCubeMesh();
            material.SetColor("_Color",new Color(1f,1f,1f,0.2f));

            meshAxis = BuildMeshAxis();
            meshFrame = BuildFrame();

            meshPivot = BuildCubeMesh();
            SetPivotColor(new Color(1f,1f,1f,1f));

            isLoaded = true;
        }

        protected override void Render() {
            base.Render();
            if(meshAxis != null && materialForAxis != null)
                Graphics.DrawMesh(meshAxis,MvmOfScreenAxis,materialForAxis,gameObject.layer);
            if(meshFrame != null && materialForAxis != null)
                Graphics.DrawMesh(meshFrame,Mvm,materialForAxis,gameObject.layer);
            if(meshPivot != null && materialForPivot != null)
                Graphics.DrawMesh(meshPivot,MvmOfPivot,materialForPivot,gameObject.layer);
        }

        protected Mesh BuildFrame() {
            Vector3[] vertices = new Vector3[] {
                new Vector3 (-0.5f, -0.5f,0f),
                new Vector3 (-0.5f, 0.5f,0f),
                new Vector3 (0.5f, 0.5f,0f),
                new Vector3 (0.5f, -0.5f,0f),
            };

            Color[] colors = new Color[]{
                Color.magenta,
                Color.magenta,
                Color.magenta,
                Color.magenta,
            };

            int[] indices = new int[]{0,1, 1,2, 2,3, 3,0};

            var mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.colors = colors;
            mesh.SetIndices(indices,MeshTopology.Lines,0);
            return mesh;
        }

        protected Mesh BuildMeshAxis() {
            Vector3[] vertices = new Vector3[] {
                new Vector3 (-0.15f, 0f,0f),
                new Vector3 (0.15f, 0f,0f),
                new Vector3 (0f, -0.15f,0f),
                new Vector3 (0f, 0.15f,0f),
                new Vector3 (0f, 0f,-0.3f),
                new Vector3 (0f, 0f,0.3f),
            };

            Color[] colors = new Color[]{
                Color.red,
                Color.red,
                Color.green,
                Color.green,
                Color.yellow,
                Color.yellow,
            };

            int[] indices = new int[]{0,1,2,3,4,5};

            var mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.colors = colors;
            mesh.SetIndices(indices,MeshTopology.Lines,0);
            return mesh;
        }

        protected Mesh BuildCubeMesh() {
            Vector3[] vertices = new Vector3[] {
                new Vector3 (-0.5f, -0.5f, -0.5f),
                new Vector3 ( 0.5f, -0.5f, -0.5f),
                new Vector3 ( 0.5f,  0.5f,  -0.5f),
                new Vector3 (-0.5f,  0.5f,  -0.5f),
                new Vector3 (-0.5f,  0.5f,  0.5f),
                new Vector3 ( 0.5f,  0.5f,  0.5f),
                new Vector3 ( 0.5f, -0.5f,  0.5f),
                new Vector3 (-0.5f, -0.5f,  0.5f),
            };
            int[] triangles = new int[] {
                0, 2, 1,
                0, 3, 2,
                2, 3, 4,
                2, 4, 5,
                1, 2, 5,
                1, 5, 6,
                0, 7, 4,
                0, 4, 3,
                5, 4, 7,
                5, 7, 6,
                0, 6, 7,
                0, 1, 6
            };

            var mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            return mesh;
        }
    }
}