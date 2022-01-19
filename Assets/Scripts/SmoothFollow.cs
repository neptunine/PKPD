using System;
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

        [Serializable]
        public class XYZ
        {
            public bool
                X,
                Y,
                Z;
        }

        public XYZ freezePosition = new XYZ();

        public Transform
            anchor;

        public float
            lerpSpeed = 3f,
            smoothTime = .3f;
            
        private float
            _velocityX,
            _velocityY,
            _velocityZ;

        private void Update()
        {
            if (anchor == null || mode == modeSetting.None) return;

            Vector3 newPos = transform.position;

            switch (mode)
            {
                case modeSetting.None:
                    break;

                case modeSetting.Lerp:
                    if (!freezePosition.X)
                        newPos.x = Mathf.Lerp(transform.position.x, anchor.position.x, Time.deltaTime * lerpSpeed);
                    if (!freezePosition.Y)
                        newPos.y = Mathf.Lerp(transform.position.y, anchor.position.y, Time.deltaTime * lerpSpeed);
                    if (!freezePosition.Z)
                        newPos.z = Mathf.Lerp(transform.position.z, anchor.position.z, Time.deltaTime * lerpSpeed);
                    break;

                case modeSetting.SmoothDamp:
                    if (!freezePosition.X)
                        newPos.x = Mathf.SmoothDamp(transform.position.x, anchor.position.x, ref _velocityX, smoothTime);
                    if (!freezePosition.Y)
                        newPos.y = Mathf.SmoothDamp(transform.position.y, anchor.position.y, ref _velocityY, smoothTime);
                    if (!freezePosition.Z)
                        newPos.z = Mathf.SmoothDamp(transform.position.z, anchor.position.z, ref _velocityZ, smoothTime);
                    break;

                default:
                    break;
            }

            transform.position = newPos;
        }
    }
}
