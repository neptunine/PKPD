using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class SideMenu : MonoBehaviour
    {
        public Vector2
            initalLocation,
            targetLocation;

        public float
            duration = .5f;

        private RectTransform
            _rect;

        private float
            _timer = 0f;

        private bool
            _moving = false,
            _state = false;

        private void Start()
        {
            transform.parent.gameObject.SetActive(false);
            _rect = GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (!_moving) return;

            if (_timer <= duration && _timer >= 0)
            {
                if (_state) _timer -= Time.deltaTime;
                else _timer += Time.deltaTime;
                float t = _timer / duration;
                float x = Mathf.SmoothStep(initalLocation.x, targetLocation.x, t);
                float y = Mathf.SmoothStep(initalLocation.y, targetLocation.y, t);
                _rect.anchoredPosition = new Vector2(x, y);
            }
            else
            {
                if (_state) _timer = 0;
                else _timer = duration;
                _moving = false;
                if (_state) transform.parent.gameObject.SetActive(false);
            }
        }

        public void Show()
        {
            _rect.anchoredPosition = initalLocation;
            transform.parent.gameObject.SetActive(true);
            _moving = true;
            _state = false;
        }

        public void Hide()
        {
            _rect.anchoredPosition = targetLocation;
            _moving = true;
            _state = true;
        }
    }
}