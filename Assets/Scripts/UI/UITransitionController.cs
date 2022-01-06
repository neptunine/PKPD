using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UITransitionController : MonoBehaviour
    {
        [Tooltip("Set inital position to zero and disable timeout")]
        public bool
            initalIsZero = false;

        [Tooltip("timeout * smoothTime")]
        public int
            timeout = 30;

        private RectTransform
            _initalObject,
            _targetObject;

        private Vector2
            _inital,
            _spacing;

        private float
            _duration,
            _timer;

        private bool
            _moving = false;

        private void Update()
        {
            if (_moving)
            {
                if (_timer < _duration * timeout)
                {
                    _timer += Time.deltaTime;
                    //float t = _TransTimer / _TransDuration;
                    //_TransInitalObject.anchoredPosition = new Vector2(Mathf.SmoothStep(_TransInital.x, _TransInital.x - _TransSpacing.x, t), Mathf.SmoothStep(_TransInital.y, _TransInital.y - _TransSpacing.y, t));
                    //_TransTargetObject.anchoredPosition = new Vector2(Mathf.SmoothStep(_TransInital.x + _TransSpacing.x, _TransInital.x, t), Mathf.SmoothStep(_TransInital.y + _TransSpacing.y, _TransInital.y, t));
                    float velocityX0 = 0f, velocityY0 = 0f, velocityX1 = 0f, velocityY1 = 0f;
                    _initalObject.anchoredPosition = new Vector2(Mathf.SmoothDamp(_initalObject.anchoredPosition.x, _inital.x - _spacing.x, ref velocityX0, _duration), Mathf.SmoothDamp(_initalObject.anchoredPosition.y, _inital.y - _spacing.y, ref velocityY0, _duration));
                    _targetObject.anchoredPosition = new Vector2(Mathf.SmoothDamp(_targetObject.anchoredPosition.x, _inital.x, ref velocityX1, _duration), Mathf.SmoothDamp(_targetObject.anchoredPosition.y, _inital.y, ref velocityY1, _duration));
                }
                else
                {
                    _timer = 0f;
                    _moving = false;
                    _initalObject.anchoredPosition = _inital + _spacing;
                    _targetObject.anchoredPosition = _inital;
                    _initalObject.gameObject.SetActive(false);
                }
            }
        }
        public void UITransition(RectTransform fromObject, RectTransform toObject, Vector2 spacing, float smoothTime)
        {
            if (_moving && !initalIsZero) return;
            _initalObject = fromObject;
            _targetObject = toObject;
            _spacing = spacing;
            _duration = smoothTime;
            if (initalIsZero) _inital = Vector2.zero;
            else _inital = _initalObject.anchoredPosition;
            _moving = true;
            _targetObject.gameObject.SetActive(true);
            _targetObject.anchoredPosition = new Vector2(_inital.x + _spacing.x, _inital.y + _spacing.y);
        }
    }
}