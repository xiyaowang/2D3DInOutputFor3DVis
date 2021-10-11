using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudyMismatch2D3D.S23_Output_HoloLens {

    public class DockingTarget:ManipulableObject {

        public TextAsset obj;

        public override void Load() {
            base.Load();
            if(obj == null)
                throw new System.Exception("Null obj in " + gameObject.name);

            mesh = LoaderObj.Load(obj);

            material.SetColor("_Color",new Color(0,1,0,0.5f));

            isLoaded = true;
        }
    }
}