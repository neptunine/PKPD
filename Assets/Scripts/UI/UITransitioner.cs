using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace UI
{
    public class UITransitioner : MonoBehaviour
    {
        [Header("Transition")]
        public RectTransform initial;
        public RectTransform target;

        public Vector2 spacing;

        private SmoothFollow _initial;
        private SmoothFollow _target;

        [Header("Move")]
        public RectTransform moveTarget;

        public Vector2 movement;

        private SmoothFollow _move;

        private void Start()
        {
            if (initial)
            {
                _initial = initial.GetComponent<SmoothFollow>();
                if (!_initial) Debug.LogWarning($"SmoothFollow component not found on {initial.name}");
            }
            if (target)
            {
                _target = target.GetComponent<SmoothFollow>();
                if (!_target) Debug.LogWarning($"SmoothFollow component not found on {target.name}");
            }
            if (moveTarget)
            {
                _move = moveTarget.GetComponent<SmoothFollow>();
                if (!_move) Debug.LogWarning($"SmoothFollow component not found on {moveTarget.name}");
            }
        }

        public void Transition()
        {
            if (_initial && _target)
            {
                if (!_initial.anchor)
                {
                    Debug.LogWarning($"Anchor on SmoothFollow component not found on {initial.name}");
                    goto skip;
                }
                if (!_target.anchor)
                {
                    Debug.LogWarning($"Anchor on SmoothFollow component not found on {target.name}");
                    goto skip;
                }

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
            skip:
            if (_move)
            {
                if (!_move.anchor)
                {
                    Debug.LogWarning($"Anchor on SmoothFollow component not found on {moveTarget.name}");
                    return;
                }
                RectTransform anchor = _move.anchor.GetComponent<RectTransform>();
                anchor.anchoredPosition = new Vector2(movement.x + anchor.anchoredPosition.x, movement.y + anchor.anchoredPosition.y);
            }
        }

        IEnumerator waitForTimeout(float t)
        {
            yield return new WaitForSeconds(t);

            initial.gameObject.SetActive(false);
        }
    }
}
