using System.Collections;
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
            a,
            b;

        private SpriteRenderer
            _renderer;

        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        private void LateUpdate()
        {
            int _y = Mathf.Clamp(a, 0, Jar.Length - 1);
            int _x = Mathf.Clamp(b, 0, Jar[_y].sprites.Length - 1);
            _renderer.sprite = Jar[_y].sprites[_x];
        }

        public void nextStage()
        {
            a = Mathf.Clamp(a + 1, 0, Jar.Length - 1);
        }

        public void preStage()
        {
            a = Mathf.Clamp(a - 1, 0, Jar.Length - 1);
        }
    }
}
