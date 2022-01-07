using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UITransitionController : MonoBehaviour
    {
        [Tooltip("Timeout for user input")]
        public bool
            inputTimeout = false;

        [Tooltip("Timeout for the animation *smoothTime")]
        public int
            animationTimeout = 30;

        private RectTransform
            _initalObject,
            _targetObject;

        private Vector2
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
                if (_timer < _duration * animationTimeout)
                {
                    _timer += Time.deltaTime;
                    //float t = _TransTimer / _TransDuration;
                    //_TransInitalObject.anchoredPosition = new Vector2(Mathf.SmoothStep(_TransInital.x, _TransInital.x - _TransSpacing.x, t), Mathf.SmoothStep(_TransInital.y, _TransInital.y - _TransSpacing.y, t));
                    //_TransTargetObject.anchoredPosition = new Vector2(Mathf.SmoothStep(_TransInital.x + _TransSpacing.x, _TransInital.x, t), Mathf.SmoothStep(_TransInital.y + _TransSpacing.y, _TransInital.y, t));
                    float velocityX0 = 0f, velocityY0 = 0f, velocityX1 = 0f, velocityY1 = 0f;
                    _initalObject.anchoredPosition = new Vector2(Mathf.SmoothDamp(_initalObject.anchoredPosition.x, 0 - _spacing.x, ref velocityX0, _duration), Mathf.SmoothDamp(_initalObject.anchoredPosition.y, 0 - _spacing.y, ref velocityY0, _duration));
                    _targetObject.anchoredPosition = new Vector2(Mathf.SmoothDamp(_targetObject.anchoredPosition.x, 0, ref velocityX1, _duration), Mathf.SmoothDamp(_targetObject.anchoredPosition.y, 0, ref velocityY1, _duration));
                }
                else
                {
                    _timer = 0f;
                    _moving = false;
                    _initalObject.anchoredPosition = Vector2.zero + _spacing;
                    _targetObject.anchoredPosition = Vector2.zero;
                    _initalObject.gameObject.SetActive(false);
                }
            }
        }
        public void UITransition(RectTransform fromObject, RectTransform toObject, Vector2 spacing, float smoothTime)
        {
            if (_moving)
            {
                if (!inputTimeout)
                {
                    _initalObject.anchoredPosition = Vector2.zero + _spacing;
                    _targetObject.anchoredPosition = Vector2.zero;
                    _initalObject.gameObject.SetActive(false);
                }
                else return;
            }

            _initalObject = fromObject;
            _targetObject = toObject;
            _spacing = spacing;
            _duration = smoothTime;
            _moving = true;
            _targetObject.anchoredPosition = Vector2.zero + _spacing;
            _targetObject.gameObject.SetActive(true);
        }
    }
}