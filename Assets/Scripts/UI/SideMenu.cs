using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI
{
    [RequireComponent(typeof(SmoothFollow))]
    public class SideMenu : MonoBehaviour
    {
        private RectTransform
            _rect;

        private SmoothFollow
            _smoothFollow;

        private GameObject
            _closeButton;

        private Coroutine _coroutine;

        private float
            anchorMaxX,
            anchorMinX;

        private void Start()
        {
            transform.parent.gameObject.SetActive(false);
            _smoothFollow = GetComponent<SmoothFollow>();
            _rect = _smoothFollow.anchor.GetComponent<RectTransform>();
            _closeButton = transform.parent.GetComponentInChildren<Button>().gameObject;

            anchorMaxX = _rect.anchorMax.x;
            anchorMinX = _rect.anchorMin.x;
        }

        public void Show()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _rect.anchorMax = new Vector2(anchorMinX * -1, _rect.anchorMax.y);
            _rect.anchorMin = new Vector2(0, _rect.anchorMin.y);
            _rect.offsetMax = _rect.offsetMin = Vector2.zero;

            transform.parent.gameObject.SetActive(true);
            _closeButton.SetActive(true);
        }

        public void Hide()
        {
            _rect.anchorMax = new Vector2(anchorMaxX, _rect.anchorMax.y);
            _rect.anchorMin = new Vector2(anchorMinX, _rect.anchorMin.y);
            _rect.offsetMax = _rect.offsetMin = Vector2.zero;

            _closeButton.SetActive(false);

            float timeout;
            if (_smoothFollow.mode == SmoothFollow.modeSetting.Lerp)
                timeout = 1 / _smoothFollow.lerpSpeed * 6;
            else
                timeout = _smoothFollow.smoothTime * 6;
            _coroutine = StartCoroutine(waitForTimeout(timeout));
        }

        IEnumerator waitForTimeout(float t)
        {
            yield return new WaitForSeconds(t);

            transform.parent.gameObject.SetActive(false);
            _coroutine = null;
        }
    }
}