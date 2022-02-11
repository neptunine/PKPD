using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField]
        private RectTransform
            fillRect;

        public float value
        {
            get { return _value; }
            set
            {
                _value = Mathf.Clamp01(value);
                _from = fillRect.sizeDelta.x;
                _to = Mathf.Round(min + (max - min) * _value);
                _timer = 0;
            }
        }

        private float
            _value,
            _timer,
            _from,
            _to;

        public int
            min = 0,
            max = 100;

        public float
            duration = 1f;

        private void OnEnable()
        {
            _value = _timer = 0f;
            _from = max;
            _to = min;
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
