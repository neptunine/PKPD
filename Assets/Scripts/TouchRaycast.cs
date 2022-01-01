using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TouchInput
{
    public class TouchRaycast : MonoBehaviour
    {
        public int touchCount = 0;

        public Vector3[]
            touchedPositions;

        public GameObject[]
            touchedObjects;

        void Update()
        {
            touchCount = Input.touchCount;

            touchedPositions = new Vector3[touchCount];
            touchedObjects = new GameObject[touchCount];

            for (int i = 0; i < touchCount; i++)
            {
                touchedPositions[i] = Camera.main.ScreenToWorldPoint(Input.touches[i].position);
                Debug.DrawLine(Vector3.zero, touchedPositions[i], Color.red);

                RaycastHit2D hitInformation = Physics2D.Raycast(new Vector2(touchedPositions[i].x, touchedPositions[i].y), Camera.main.transform.forward);

                if (hitInformation.collider != null)
                {
                    touchedObjects[i] = hitInformation.transform.gameObject;
                }
            }
        }
    }
}
