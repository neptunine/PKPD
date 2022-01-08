using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public class SmoothFollow : MonoBehaviour
    {
        public enum modeSetting
        {
            None,
            Lerp,
            SmoothDamp
        };

        public modeSetting
            mode = modeSetting.None;

        public Transform
            anchor,
            anchorParent;

        public float
            lerpSpeed = 3f,
            smoothTime = .05f;
            
        private float
            _velocityX,
            _velocityY;

        private void Start()
        {
            if (anchorParent)
                anchor.SetParent(anchorParent);
        }

        private void Update()
        {
            if (anchor == null) return;

            switch (mode)
            {
                case modeSetting.None:
                    break;

                case modeSetting.Lerp:
                    transform.position = new Vector3(Mathf.Lerp(transform.position.x, anchor.position.x, Time.deltaTime * lerpSpeed), Mathf.Lerp(transform.position.y, anchor.position.y, Time.deltaTime * lerpSpeed), transform.position.z);
                    break;

                case modeSetting.SmoothDamp:
                    transform.position = new Vector3(Mathf.SmoothDamp(transform.position.x, anchor.position.x, ref _velocityX, smoothTime), Mathf.SmoothDamp(transform.position.y, anchor.position.y, ref _velocityY, smoothTime), transform.position.z);
                    break;

                default:
                    break;
            }
        }
    }
}
