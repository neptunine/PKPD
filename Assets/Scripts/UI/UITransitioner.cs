using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace UI
{
    public class UITransitioner : MonoBehaviour
    {
        public RectTransform
            initial,
            target;

        public Vector2
            spacing;

        private SmoothFollow
            _initial,
            _target;

        private void Start()
        {
            _initial = initial.GetComponent<SmoothFollow>();
            _target = target.GetComponent<SmoothFollow>();
        }

        public void Transition()
        {
            target.gameObject.SetActive(true);

            initial.anchoredPosition = Vector2.zero;
            target.anchoredPosition = new Vector2(spacing.x, spacing.y);
            _initial.anchor.GetComponent<RectTransform>().anchoredPosition = new Vector2(spacing.x * -1, spacing.y * -1);
            _target.anchor.position = Vector3.zero;

            float timeout;
            if (_initial.mode == SmoothFollow.modeSetting.Lerp)
                timeout = 1 / _initial.lerpSpeed;
            else
                timeout = _initial.smoothTime;
            StartCoroutine(waitForTimeout(timeout));
        }

        IEnumerator waitForTimeout(float t)
        {
            yield return new WaitForSeconds(t);

            initial.gameObject.SetActive(false);
        }
    }
}
