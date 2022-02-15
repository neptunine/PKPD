using System;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace Game
{
    public class CharacterHandler : MonoBehaviour
    {
        [Serializable]
        public class abcdefgg
        {
            public Sprite
                blank,
                a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z;
        }

        [SerializeField]
        private abcdefgg
            characters;

        public int
            index;

        public char
            character;

        [Min(1)]
        public int
            scale;

        private void Start()
        {
            Image _sprite = GetComponentInChildren<Image>();

            switch (character)
            {
                case 'a': case 'A': _sprite.sprite = characters.a; break;
                case 'b': case 'B': _sprite.sprite = characters.b; break;
                case 'c': case 'C': _sprite.sprite = characters.c; break;
                case 'd': case 'D': _sprite.sprite = characters.d; break;
                case 'e': case 'E': _sprite.sprite = characters.e; break;
                case 'f': case 'F': _sprite.sprite = characters.f; break;
                case 'g': case 'G': _sprite.sprite = characters.g; break;
                case 'h': case 'H': _sprite.sprite = characters.h; break;
                case 'i': case 'I': _sprite.sprite = characters.i; break;
                case 'j': case 'J': _sprite.sprite = characters.j; break;
                case 'k': case 'K': _sprite.sprite = characters.k; break;
                case 'l': case 'L': _sprite.sprite = characters.l; break;
                case 'm': case 'M': _sprite.sprite = characters.m; break;
                case 'n': case 'N': _sprite.sprite = characters.n; break;
                case 'o': case 'O': _sprite.sprite = characters.o; break;
                case 'p': case 'P': _sprite.sprite = characters.p; break;
                case 'q': case 'Q': _sprite.sprite = characters.q; break;
                case 'r': case 'R': _sprite.sprite = characters.r; break;
                case 's': case 'S': _sprite.sprite = characters.s; break;
                case 't': case 'T': _sprite.sprite = characters.t; break;
                case 'u': case 'U': _sprite.sprite = characters.u; break;
                case 'v': case 'V': _sprite.sprite = characters.v; break;
                case 'w': case 'W': _sprite.sprite = characters.w; break;
                case 'x': case 'X': _sprite.sprite = characters.x; break;
                case 'y': case 'Y': _sprite.sprite = characters.y; break;
                case 'z': case 'Z': _sprite.sprite = characters.z; break;
                default: _sprite.sprite = characters.blank; break;
            }

            _sprite.SetNativeSize();
            _sprite.GetComponent<RectTransform>().sizeDelta *= scale;
            gameObject.GetComponent<SmoothFollow>().anchor.GetComponent<RectTransform>().sizeDelta = gameObject.GetComponent<RectTransform>().sizeDelta *= scale;
        }
    }
}
