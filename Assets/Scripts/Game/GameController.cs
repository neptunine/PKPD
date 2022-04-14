using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using Utility;

namespace Game {
    public class GameController : MonoBehaviour
    {
        [SerializeField]
        private LevelController
            level;

        [SerializeField]
        private AudioController
            audioController;

        [SerializeField]
        private PlayerData
            player;

        [SerializeField]
        private LevelSelection
            levelSelect;

        [SerializeField]
        private GameObject
            menuObject,
            mainMenu,
            levelMenu,
            optionsMenu,
            creditsMenu,
            levelUI;

        [SerializeField]
        private string
            _filename;

        private string
            _filepath;

        private void Awake()
        {
            _filepath = $"{Application.streamingAssetsPath}/{_filename}";

            level.gameObject.SetActive(false);
            levelUI.SetActive(false);
            menuObject.SetActive(true);
            mainMenu.SetActive(true);
            levelMenu.SetActive(false);
            optionsMenu.SetActive(false);
            creditsMenu.SetActive(false);
        }

        private void Start()
        {
            
        }

        private void Update()
        {

        }

        public void StartLevel()
        {
            if (player.isOutOfLife)
            {
                Debug.Log($"[<color=magenta>GameController</color>] Can't Start Level without life");
                return;
            }
            
            menuObject.SetActive(false);
            level.gameObject.SetActive(true);
            levelUI.SetActive(true);

            //string[] words = wordFile[mode].text.Split("\n"[0]);
            //for (int i = 0; i < words.Length; i++)
            //{
            //    string tmp = words[i];
            //    int r = Random.Range(i, words.Length);
            //    words[i] = words[r];
            //    words[r] = tmp;
            //}

            //string[] words = GetWords(20, 20);
            //level.Initialize(words);

            string[] words = GetWords(levelSelect.topic, Mathf.FloorToInt(Random.Range(levelSelect.wordMin, levelSelect.wordMax)));
            level.Initialize(words, levelSelect.hide, !levelSelect.manual);

            Debug.Log($"[<color=magenta>GameController</color>] Started Level with topic {levelSelect.topic} and mode {levelSelect.difficulty}");
        }

        public void TerminateLevel()
        {
            level.gameObject.SetActive(false);
            levelUI.SetActive(false);
            menuObject.SetActive(true);

            Debug.Log($"[<color=magenta>GameController</color>] Ended Level");
        }

        public string[] GetWords(int topic, int quantity)
        {
            string commandString = $"select * from Words WHERE GenreID = {topic} order by RANDOM() LIMIT {quantity};";
            string[] wordarray = new string[quantity];

            using (var connection = new SqliteConnection("Data Source=" + _filepath))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandString;

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        for (int i = 0; reader.Read(); i++)
                        {
                            wordarray[i] = (string)reader["Name"];

                        }
                        reader.Close();
                    }
                }
                connection.Close();
            }
            Debug.Log($"[<color=magenta>LevelController</color>] Read word from \"{_filepath}\" with commond \"{commandString}\"");
            return wordarray;
        }
    }
}