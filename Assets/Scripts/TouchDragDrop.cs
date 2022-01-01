using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TouchInput
{
    public class TouchDragDrop : MonoBehaviour
    {
        [Tooltip("Use last instead of first when have multiple input")]
        public bool
            last = false;

        [Tooltip("Object following speed")]
        public float
            speed = 3f;

        [Tooltip("Moveable objects")]
        public GameObject[]
            targetObjects;

        [Tooltip("Object currently picked up")]
        public GameObject
            pickedObject;

        private Vector3
            offsetPos;

        void Update()
        {
            if (Input.touchCount > 0)
            {
                Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(last ? -1 : 0).position);

                if (Input.GetTouch(last ? -1 : 0).phase == TouchPhase.Began)
                {
                    RaycastHit2D hitInformation = Physics2D.Raycast(new Vector2(touchPos.x, touchPos.y), Camera.main.transform.forward);

                    if (hitInformation.collider != null && Array.IndexOf(targetObjects, hitInformation.transform.gameObject) > -1)
                    {

                        pickedObject = hitInformation.transform.gameObject;
                        offsetPos = touchPos - pickedObject.transform.position;
                        Debug.Log($"[{this.name}] picked up {pickedObject.name}");
                    }
                }
                if (Input.GetTouch(last ? -1 : 0).phase == TouchPhase.Ended && pickedObject != null)
                {
                    Debug.Log($"[{this.name}] dropped {pickedObject.name}");
                    pickedObject = null;
                    offsetPos = Vector3.zero;
                }

                if (pickedObject != null)
                {
                    pickedObject.transform.position = Vector3.Lerp(pickedObject.transform.position, new Vector3(touchPos.x - offsetPos.x, touchPos.y - offsetPos.y, pickedObject.transform.position.z), Time.deltaTime * speed);
                }
            }
        }
    }
}
