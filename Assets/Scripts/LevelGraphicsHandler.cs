﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class LevelGraphicsHandler : MonoBehaviour
    {
        [System.Serializable]
        public class SpriteArray
        {
            public Sprite[] sprites;
        }

        public SpriteArray[]
            Jar;

        public int
            row,
            column;

        private SpriteRenderer
            _renderer;

        public bool
            isFinal = false;
            
        private float
            current,
            velocity = 0f;

        [Min(0)]
        public float
            rotateStep = .3f,
            elasticity = 40f,
            damping = .5f;

        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
            Input.gyro.enabled = true;
        }

        private void Update()
        {
            SetRotation();

            isFinal = row == Jar.Length - 1;
        }

        private void LateUpdate()
        {
            int _y = Mathf.Clamp(row, 0, Jar.Length - 1);
            int _x = Mathf.Clamp(column, 0, Jar[_y].sprites.Length - 1);
            _renderer.sprite = Jar[_y].sprites[_x];

        }

        public void nextStage()
        {
            row = Mathf.Clamp(row + 1, 0, Jar.Length - 1);
            isFinal = row == Jar.Length - 1;
        }

        public void preStage()
        {
            row = Mathf.Clamp(row - 1, 0, Jar.Length - 1);
        }

        private void SetRotation()
        {
            float target = Mathf.Clamp(Input.gyro.gravity.x, rotateStep * -3f, rotateStep * 3f);
            float force = elasticity * (target - current) - damping * velocity;
            velocity = velocity + force * Time.deltaTime;
            current = current + velocity * Time.deltaTime;

            int index = Mathf.Clamp(Mathf.RoundToInt(current / rotateStep), -4, 4);
            if (index < 0)
                index *= -1;
            else if (index > 0)
                index += 4;
            column = index;
        }
    }
}
