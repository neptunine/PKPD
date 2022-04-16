using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class LevelGraphicsHandler : MonoBehaviour
    {
        [System.Serializable]
        public class SpriteArray
        {
            public Sprite[] sprites;
        }

        public bool isFinal
        {
            get { return _stage == jar.Length - 1; }
        }

        public int maxstage
        {
            get { return jar.Length - 1; }
        }

        [SerializeField]
        private SpriteRenderer
            jarRenderer;

        [SerializeField]
        private SpriteArray[]
            jar;

        private int
            _stage;

        private float
            _tilt;

        private Animator
            _animator;
            
        private float
            _current,
            _velocity = 0f;

        [Min(0)]
        public float
            rotateStep = .3f,
            elasticity = 40f,
            damping = .5f;

#if UNITY_EDITOR
        [Header("Debug")]

        public bool Override = false;
        [Range(-1, 7)] public int stage;
        [Range(-4, 4)] public float tilt;
#endif

        private void Awake()
        {
            Input.gyro.enabled = true;
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            
        }

        private void Update()
        {
            
        }

        private void LateUpdate()
        {
#if UNITY_EDITOR
            if (Override)
            {
                _stage = stage;
                _tilt = tilt;
                _animator.SetInteger("stage", _stage);
                _animator.SetFloat("tilt", _tilt);
            }
            else
                GetRotation();
#else
            GetRotation();
#endif
            int _y = Mathf.Clamp(_stage, 0, jar.Length - 1);
            int _x = Mathf.RoundToInt(_tilt);
            if (_x < 1)
                _x *= -1;
            else
                _x += 4;
            _x = Mathf.Clamp(_x, 0, jar[_y].sprites.Length - 1);

            jarRenderer.sprite = jar[_y].sprites[_x];
        }

        public void SetStage(int i)
        {
            if (i == -1)
            {
                _animator.SetInteger("stage", -1);
                return;
            }
            _stage = Mathf.Clamp(jar.Length - 1 - i, 0, jar.Length - 1);
            _animator.SetInteger("stage", _stage);
        }

        public void NextStage()
        {
            _stage = Mathf.Min(_stage + 1, jar.Length - 1);
            _animator.SetInteger("stage", _stage);
        }

        public void PreStage()
        {
            _stage = Mathf.Max(_stage - 1, 0);
            _animator.SetInteger("stage", _stage);
        }

        public void Clear()
        {
            _stage = 0;
            _animator.SetInteger("stage", 0);
            _animator.SetFloat("tilt", 0);
        }

        private void GetRotation()
        {
            float target = Mathf.Clamp(Input.gyro.gravity.x, rotateStep * -3f, rotateStep * 3f);
            float force = elasticity * (target - _current) - damping * _velocity;
            _velocity = _velocity + force * Time.deltaTime;
            _current = _current + _velocity * Time.deltaTime;

            _tilt = Mathf.Clamp(_current / rotateStep, -4, 4);
            _animator.SetFloat("tilt", _tilt);
        }
    }
}
