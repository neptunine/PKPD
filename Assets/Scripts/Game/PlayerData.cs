using System;
using System.IO;
using UnityEngine;

namespace Game
{
    public class PlayerData : MonoBehaviour
    {
        [SerializeField]
        private string
            _filename;

        private string
            _filepath;

        public bool
            json = false;

        [Serializable]
        private class Save
        {
            public int
                lives;

            public long
                damageTime,
                nextHealTime;

            public int
                level,
                currentExp,
                totalExp;
        }

        [Header("Lives")]
        [SerializeField]
        private Animator
            _livesDisplay;

        public int
            fullLives = 10,
            healTime = 600000;

        private int
           _lives,
           _timeToHeal;

        private DateTime
            _damageTime,
            _nextHealTime;

        public bool isOutOfLife
        {
            get { return _lives <= 0; }
        }

        public int lives
        {
            get { return _lives; }
        }


        [Header("level")]
        private int
            _level,
            _currentExp,
            _expForNextLevel,
            _totalExp;

        public int level
        {
            get { return _level; }
        }

        public int totalExp
        {
            get { return _totalExp; }
        }

        void Awake()
        {
            _filepath = $"{Application.persistentDataPath}/{_filename}";
            ReadSave();

        }

        private void Update()
        {
            if (_lives < fullLives)
            {
                DateTime now = DateTime.UtcNow;
                _timeToHeal = Mathf.RoundToInt((float)(_nextHealTime - now).TotalMilliseconds);

                if (_timeToHeal < 0)
                {
                    _lives = Mathf.Clamp(_lives + 1, 0, fullLives);

                    if (_lives < fullLives)
                    {
                        _nextHealTime = now.Add(TimeSpan.FromMilliseconds(healTime + _timeToHeal));
                        _timeToHeal = Mathf.RoundToInt((float)(_nextHealTime - now).TotalMilliseconds);

                        Debug.Log($"[<color=orange>PlayerData</color>] Healed [{_lives}/{fullLives}]");
                    }
                    else
                    {
                        _timeToHeal = 0;

                        Debug.Log($"[<color=orange>PlayerData</color>] Fully healed [{_lives}/{fullLives}]");
                    }

                    WriteSave();
                }
            }

            _livesDisplay.SetInteger("lives", _lives);
        }

        public void Damage()
        {
            DateTime now = DateTime.UtcNow;
            _damageTime = now;
            _lives = Mathf.Clamp(_lives - 1, 0, fullLives);

            if (_lives <= 0)
            {
                Debug.Log($"[<color=orange>PlayerData</color>] Out of lives [{_lives}/{fullLives}]");
                _lives = 0;
            }
            else if (_lives == fullLives - 1)
            {
                _nextHealTime = now.Add(TimeSpan.FromMilliseconds(healTime));
                _timeToHeal = healTime;
            }

            WriteSave();
        }

        public void AddExperience(int exp)
        {
            _expForNextLevel = GetExpforLevel(_level);
            _totalExp += exp;
            _currentExp += exp;
            Debug.Log($"[<color=orange>PlayerData</color>] Gained {exp} experience [lv.{_level}: {Math.Min(_currentExp, _expForNextLevel)}/{_expForNextLevel}]");
            
            while (_currentExp >= _expForNextLevel)
            {
                _currentExp -= _expForNextLevel;
                _level += 1;
                _expForNextLevel = GetExpforLevel(_level);

                Debug.Log($"[<color=orange>PlayerData</color>] Level {_level} reached [lv.{_level}: {Math.Min(_currentExp, _expForNextLevel)}/{_expForNextLevel}]");
            }

            WriteSave();
        }

        private int GetExpforLevel(int n)
        {
            return Mathf.RoundToInt((4 * Mathf.Pow(n + 5, 3)) / 5);
        }

        private void WriteSave()
        {
            Save save = new Save
            {
                lives = _lives,
                damageTime = ((DateTimeOffset)_damageTime).ToUnixTimeMilliseconds(),
                nextHealTime = ((DateTimeOffset)_nextHealTime).ToUnixTimeMilliseconds(),
                level = _level,
                currentExp = _currentExp,
                totalExp = _totalExp,
            };
            if (json)
                File.WriteAllText(_filepath, JsonUtility.ToJson(save));
            else
                WriteToBinaryFile<Save>(_filepath, save);

        }

        private void ReadSave()
        {
            if (json)
            if (File.Exists(_filepath))
            {
                Save save = JsonUtility.FromJson<Save>(File.ReadAllText(_filepath));
                _lives = save.lives;
                _damageTime = DateTimeOffset.FromUnixTimeMilliseconds(save.damageTime).UtcDateTime;
                _nextHealTime = DateTimeOffset.FromUnixTimeMilliseconds(save.nextHealTime).UtcDateTime;
                _level = save.level;
                _currentExp = save.currentExp;
                _totalExp = save.totalExp;

                Debug.Log($"[<color=orange>PlayerData</color>] Read file \"{_filepath}\"");
            }
            else
            {
                DateTime now = DateTime.UtcNow;
                _lives = fullLives;
                _damageTime = now;
                _nextHealTime = now;
                _level = _currentExp = _totalExp = 0;

                Debug.Log($"[<color=orange>PlayerData</color>] File not found \"{_filepath}\"");
            }
            else
            try
            {
                Save save = ReadFromBinaryFile<Save>(_filepath);
                _lives = save.lives;
                _damageTime = DateTimeOffset.FromUnixTimeMilliseconds(save.damageTime).UtcDateTime;
                _nextHealTime = DateTimeOffset.FromUnixTimeMilliseconds(save.nextHealTime).UtcDateTime;
                _level = save.level;
                _currentExp = save.currentExp;
                _totalExp = save.totalExp;

                Debug.Log($"[<color=orange>PlayerData</color>] Read file \"{_filepath}\"");
            }
            catch (Exception e)
            {
                DateTime now = DateTime.UtcNow;
                _lives = fullLives;
                _damageTime = now;
                _nextHealTime = now;
                _level = _currentExp = _totalExp = 0;

                Debug.LogWarning($"[<color=orange>PlayerData</color>] File \"{_filepath}\" can not be read\n{e}");
            }
        }


        /// <summary>
        /// Writes the given object instance to a binary file.
        /// <para>Object type (and all child types) must be decorated with the [Serializable] attribute.</para>
        /// <para>To prevent a variable from being serialized, decorate it with the [NonSerialized] attribute; cannot be applied to properties.</para>
        /// </summary>
        /// <typeparam name="T">The type of object being written to the binary file.</typeparam>
        /// <param name="filePath">The file path to write the object instance to.</param>
        /// <param name="objectToWrite">The object instance to write to the binary file.</param>
        /// <param name="append">If false the file will be overwritten if it already exists. If true the contents will be appended to the file.</param>
        public static void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = false)
        {
            using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, objectToWrite);
            }
        }

        /// <summary>
        /// Reads an object instance from a binary file.
        /// </summary>
        /// <typeparam name="T">The type of object to read from the binary file.</typeparam>
        /// <param name="filePath">The file path to read the object instance from.</param>
        /// <returns>Returns a new instance of the object read from the binary file.</returns>
        public static T ReadFromBinaryFile<T>(string filePath)
        {
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return (T)binaryFormatter.Deserialize(stream);
            }
        }
    }
}
