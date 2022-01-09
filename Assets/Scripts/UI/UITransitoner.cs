using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace UI
{
    public class UITransitoner : MonoBehaviour
    {
        public RectTransform
            initial,
            target;

        public Vector2
            spacing;

        public float
            timeout = 1f;

        private SmoothFollow
            _initial,
            _target;

        private void Start()
        {
            _initial.enabled = _target.enabled = false;
        }

        public void Transition()
        {
            target.gameObject.SetActive(true);

            _initial = initial.GetComponent<SmoothFollow>();
            _target = target.GetComponent<SmoothFollow>();


            initial.anchoredPosition = Vector2.zero;
            target.anchoredPosition = new Vector2(spacing.x, spacing.y);
            _initial.anchor.GetComponent<RectTransform>().anchoredPosition = new Vector2(spacing.x * -1, spacing.y * -1);
            _target.anchor.position = Vector3.zero;

            StartCoroutine(waitForTimeout());
        }

        IEnumerator waitForTimeout()
        {
            yield return new WaitForSeconds(timeout);

            initial.gameObject.SetActive(false);
        }
    }
}
