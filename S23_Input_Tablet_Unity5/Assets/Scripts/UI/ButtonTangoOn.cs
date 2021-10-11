using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace StudyMismatch2D3D.S23_Input_Android_Unity5 {

    [RequireComponent(typeof(Button))]
    public class ButtonTangoOn:MonoBehaviour/*, IPointerDownHandler, IPointerUpHandler*/ {
        //public void OnPointerUp(PointerEventData eventData) {
        //    GlobalManager.Instance.IsTangoOn = false;
        //}
        //public void OnPointerDown(PointerEventData eventData) {
        //    GlobalManager.Instance.IsTangoOn = true;
        //}

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {
            if(Input.touchCount == 0) {
                GlobalManager.Instance.IsTangoOn = false;
                return;
            }
            if(EventSystem.current.currentSelectedGameObject != gameObject || !EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId)) {
                GlobalManager.Instance.IsTangoOn = false;
                return;
            }

            GlobalManager.Instance.IsTangoOn = true;

        }
    }
}