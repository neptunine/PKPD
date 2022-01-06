﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UITransition : MonoBehaviour
    {
        [SerializeField]
        UITransitionController
            controller;

        public RectTransform
            inital,
            target;

        public Vector2
            spacing;

        public float
            smoothTime;

        public void Transition()
        {
            controller.UITransition(inital, target, spacing, smoothTime);
        }
    }
}
