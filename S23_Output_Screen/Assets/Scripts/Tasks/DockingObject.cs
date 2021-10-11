using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudyMismatch2D3D.S23_Output_Screen {

    public class DockingObject:ManipulableObject {

        public TextAsset obj;

        protected Mesh meshOfScreenAxis;

        public Shader shaderOfAxis;
        protected Material materialOfAxis;
        protected Vector3 scalingOfAxis = Vector3.one*0.3f;
        protected Matrix4x4 MvmOfScreenAxis;

        protected override void Awake() {
            base.Awake();
            if(shaderOfAxis != null) {
                materialOfAxis = new Material(shaderOfAxis);
            } else {
                throw new System.Exception("Null shader in " + gameObject.name);
            }
        }

        public override void ComputeMvm() {
            base.ComputeMvm();
            MvmOfScreenAxis = Matrix4x4.TRS(position,Quaternion.identity,scalingOfAxis);
        }

        protected override void Render() {
            base.Render();
            if(meshOfScreenAxis != null && materialOfAxis != null)
                Graphics.DrawMesh(meshOfScreenAxis,MvmOfScreenAxis,materialOfAxis,gameObject.layer);

        }

        public override void Load() {
            base.Load();
            if(obj == null)
                throw new System.Exception("Null obj in " + gameObject.name);

            mesh = LoaderObj.Load(obj);
            material.SetColor("_Color",Color.white);

            meshOfScreenAxis = BuildMeshOfScreenAxis();


            isLoaded = true;
        }

        public Mesh BuildMeshOfScreenAxis() {
            Vector3[] vertices = new Vector3[] {
                new Vector3 (-0.5f, 0f,0f),
                new Vector3 (0.5f, 0f,0f),
                new Vector3 (0f, -0.5f,0f),
                new Vector3 (0f, 0.5f,0f),
                new Vector3 (0f, 0f,-0.5f),
                new Vector3 (0f, 0f,0.5f),
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


    }
}