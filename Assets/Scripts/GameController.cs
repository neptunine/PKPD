using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    public class GameController : MonoBehaviour
    {
        [SerializeField]
        GameObject
            menuObject,
            mainMenu,
            levelMenu,
            optionsMenu,
            creditsMenu,
            level,
            levelUI;

        private LevelController
            levelController;

        public TextAsset[]
            wordFile;

        private LevelController
            currentLevel;

        public int
            live;

        public int
            ss;

        private void Awake()
        {
            levelController = level.GetComponent<LevelController>();
            level.gameObject.SetActive(false);
            levelUI.gameObject.SetActive(false);
            menuObject.SetActive(true);
            mainMenu.SetActive(true);
            levelMenu.SetActive(false);
            optionsMenu.SetActive(false);
            creditsMenu.SetActive(false);
        }

        private void Start()
        {
            live = 5;
        }

        private void Update()
        {

        }

        public void StartLevel(int mode)
        {
            menuObject.SetActive(false);
            level.SetActive(true);
            levelUI.SetActive(true);
            string[] words = wordFile[mode].text.Split("\n"[0]);
            for (int i = 0; i < words.Length; i++)
            {
                string tmp = words[i];
                int r = Random.Range(i, words.Length);
                words[i] = words[r];
                words[r] = tmp;
            }
            levelController.wordList = words;
            levelController.Initialize();
        }

        public void TerminateLevel(int score)
        {
            ss = score;
            level.SetActive(false);
            levelUI.SetActive(false);
            menuObject.SetActive(true);
        }

    }
}