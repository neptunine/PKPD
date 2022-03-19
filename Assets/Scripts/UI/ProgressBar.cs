using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public class ProgressBar : MonoBehaviour
    {
        public RectTransform
            fillRect;

        //public int
        //    min = 0,
        //    max = 100;

        public int
            min,
            padding;

        public float
            duration = 1f;

        public float value
        {
            get { return _value; }
            set
            {
                _value = Mathf.Clamp01(value);
                _from = fillRect.sizeDelta.x;
                //_to = Mathf.Round(min + (max - min) * _value);
                _to = Mathf.Round(-_width + min + (padding * -2 + _width + min) * _value);
                _timer = 0;
            }
        }

        private float
            _value,
            _timer,
            _from,
            _to,
            _width;

        private void OnEnable()
        {
            _value = _timer = 0f;
            ////_from = max;
            //_to = min;
            _width = GetComponent<RectTransform>().rect.width;
            _from = padding * -2;
            _to = -_width + min;
        }

        private void Update()
        {
            if (_timer < duration)
            {
                _timer += Time.deltaTime;
                float t = Mathf.Clamp01(_timer / duration);
                t = (Mathf.Sin(t * Mathf.PI * (.2f + 2.5f * t * t * t)) * Mathf.Pow(1f - t, 2.2f) + t) * (1f + (1.2f * (1f - t)));
                fillRect.sizeDelta = new Vector2(Mathf.Round(_from + (_to - _from) * t), fillRect.sizeDelta.y);
            }
        }
    }
}
