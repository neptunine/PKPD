using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class LifeHandler : MonoBehaviour
    {
        [SerializeField]
        Text
            text;

        [Serializable]
        public class Life
        {
            public int
                lives;
            public long
                damageTime,
                nextHealTime,
                fullHealTime;
        }

        public Life
            save = new Life();

        public bool isOutOfLife
        {
            get { return _lives <= 0; }
        }

        public int lives
        {
            get { return _lives; }
        }

        public int
            timeToHeal,
            fullLives = 5,
            healTime = 3600000;

        private int
           _lives;

        private DateTime
            _damageTime,
            _nextRegeneTime,
            _fullRegeneTime;

        private void Start()
        {
            _lives = fullLives;
            _damageTime = _nextRegeneTime = _fullRegeneTime = DateTime.UtcNow;
        }

        private void Update()
        {
            if (_lives < fullLives)
            {
                DateTime now = DateTime.UtcNow;
                timeToHeal = Mathf.RoundToInt((float)(_nextRegeneTime - now).TotalMilliseconds);

                if (timeToHeal < 0)
                {
                    _lives = Mathf.Clamp(_lives + 1, 0, fullLives);

                    if (_lives < fullLives)
                    {
                        _nextRegeneTime = now.Add(TimeSpan.FromMilliseconds(healTime + timeToHeal));
                        timeToHeal = Mathf.RoundToInt((float)(_nextRegeneTime - now).TotalMilliseconds);
                        //_fullRegeneTime = now.Add(TimeSpan.FromMilliseconds(regeneTime * Mathf.Clamp(fullLives - _lives - 1, 0, fullLives) + timeToRegen));

                        Debug.Log($"[<color=red>LifeHandler</color>] Healed");
                    }
                    else
                    {
                        timeToHeal = 0;

                        Debug.Log($"[<color=red>LifeHandler</color>] Fully healed");
                    }

                    save.lives = _lives;
                    save.damageTime = ((DateTimeOffset)_damageTime).ToUnixTimeMilliseconds();
                    save.nextHealTime = ((DateTimeOffset)_nextRegeneTime).ToUnixTimeMilliseconds();
                    save.fullHealTime = ((DateTimeOffset)_fullRegeneTime).ToUnixTimeMilliseconds();
                }
            }

            text.text = _lives.ToString();
        }

        private void OnDisable()
        {

        }

        public void Damage()
        {
            DateTime now = DateTime.UtcNow;
            _damageTime = now;
            _lives = Mathf.Clamp(_lives - 1, 0, fullLives);

            if (_lives <= 0)
            {
                Debug.Log($"[<color=red>LifeHandler</color>] Out of lives");
                _lives = 0;
            }
            else if (_lives == fullLives - 1)
            {
                _nextRegeneTime = _fullRegeneTime = now.Add(TimeSpan.FromMilliseconds(healTime));
                timeToHeal = healTime;
            }
            else
            {
                _fullRegeneTime = now.Add(TimeSpan.FromMilliseconds(healTime * (fullLives - _lives - 1))).Add(_nextRegeneTime - now);
            }

            save.lives = _lives;
            save.damageTime = ((DateTimeOffset)_damageTime).ToUnixTimeMilliseconds();
            save.nextHealTime = ((DateTimeOffset)_nextRegeneTime).ToUnixTimeMilliseconds();
            save.fullHealTime = ((DateTimeOffset)_fullRegeneTime).ToUnixTimeMilliseconds();
        }
    }
}
