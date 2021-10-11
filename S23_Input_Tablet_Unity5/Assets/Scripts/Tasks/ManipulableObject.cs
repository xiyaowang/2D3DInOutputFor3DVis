using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudyMismatch2D3D.S23_Input_Android_Unity5 {

    public abstract class ManipulableObject:MonoBehaviour {

        protected Vector3 position = new Vector3(0,0,2);
        public virtual Vector3 Position {
            get { return position; }
            set {
                position = value;
                ComputeMvm();
            }
        }

        protected Quaternion rotation = Quaternion.identity;
        public virtual Quaternion Rotation {
            get { return rotation; }
            set {
                rotation = value;
                ComputeMvm();
            }
        }

        protected Vector3 scaling = Vector3.one * 0.05f;
        public virtual Vector3 Scaling {
            get { return scaling; }
            set {
                scaling = value;
                ComputeMvm();
            }
        }

        public Vector3 Translate(Vector3 distance) {
            Position += distance;
            return position;
        }

        public Quaternion Rotate(Quaternion rot) {
            Rotation = Q.Normalize(rot * Rotation);
            return Rotation;
        }

        public Vector3 Scale(float s) {
            Scaling *= s;
            return Scaling;
        }

        public Vector3 Scale(Vector3 s) {
            Scaling = Vector3.Scale(Scaling,s);
            return Scaling;
        }

        public Matrix4x4 Mvm { get; protected set; }

        public virtual void ComputeMvm() {
            Mvm = Matrix4x4.TRS(position,rotation,scaling);
        }

        public Shader shader;
        protected Mesh mesh;
        protected Material material;

        protected virtual void Render() {
            if(mesh != null && material != null)
                Graphics.DrawMesh(mesh,Mvm,material,gameObject.layer);
        }


        protected virtual void Awake() {
            if(shader != null) {
                material = new Material(shader);
            } else {
                throw new System.Exception("Null shader in " + gameObject.name);
            }
            ComputeMvm();
        }

        protected virtual void Start() {
        }

        protected virtual void Update() {

        }

        protected virtual void LateUpdate() {
            //Render();
        }

        protected virtual void OnDestroy() {
            Destroy(material);
        }

        protected bool isLoaded = false;
        public virtual void Load() {
            if(isLoaded == true)
                return;
        }
        public float distanceToPlane(Vector3 planePosition,Vector3 planeNormal,Vector3 pointInWorld) {
            Vector3 w = -(planePosition - pointInWorld);
            float res = (planeNormal.x * w.x +  planeNormal.y * w.y + planeNormal.z * w.z)
                    / Mathf.Sqrt(planeNormal.x * planeNormal.x +
                        planeNormal.y * planeNormal.y +
                        planeNormal.z * planeNormal.z);
            return res;
        }
    }
}