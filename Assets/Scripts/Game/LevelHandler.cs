using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class LevelHandler : MonoBehaviour
    {
        public int
            _level,
            _currentExp,
            _expForNextLevel,
            _totalexp;


        private void Start()
        {
            // load from db
            _level = _currentExp = _totalexp = 0;
            _expForNextLevel = GetExpforLevel(_level);
        }

        private void Update()
        {
            _expForNextLevel = GetExpforLevel(_level);
        }

        private void OnDisable()
        {
            // save to db
        }

        public void AddExperience(int exp)
        {
            _expForNextLevel = GetExpforLevel(_level);
            _totalexp += exp;
            int newExp = _currentExp + exp;
            if (newExp < _expForNextLevel)
            {
                _currentExp = newExp;

                Debug.Log($"[<color=orange>LevelHandler</color>] Gained {exp} experience");
            }
            else
            {
                _level += 1;
                _currentExp = newExp - _expForNextLevel;
                _expForNextLevel = GetExpforLevel(_level);

                Debug.Log($"[<color=orange>LevelHandler</color>] Gained {exp} experience, level {_level} reached");
            }
            
        }

        private int GetExpforLevel(int n)
        {
            return Mathf.RoundToInt((4 * Mathf.Pow(n + 5, 3)) / 5);
        }
    }
}
