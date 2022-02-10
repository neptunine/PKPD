using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public class ProgressBar : MonoBehaviour
    {
        public RectTransform
            fillRect;

        public float
            minValue = 0,
            maxValue = 1,
            value;

        void Start()
        {

        }

        void Update()
        {
            value = Mathf.Clamp(value, minValue, maxValue);
            fillRect.localScale = new Vector3(value, fillRect.localScale.y, fillRect.localScale.z);
        }
    }
}
