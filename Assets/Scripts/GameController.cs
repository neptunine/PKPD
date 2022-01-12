using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    public class GameController : MonoBehaviour
    {
        [SerializeField]
        GameObject
            mainMenu,
            level;

        public TextAsset[]
            wordFile;

        private GameObject
            currentLevel;

        private LevelController
            currentLevelController;

        public int
            ss;

        private void Start()
        {

        }

        private void Update()
        {

        }

        public void StartLevel(int mode)
        {
            mainMenu.SetActive(false);
            currentLevel = Instantiate(level);
            currentLevelController = currentLevel.GetComponent<LevelController>();
            currentLevelController.wordFile = wordFile[mode];
            currentLevel.SetActive(true);
        }

        public void TerminateLevel(int score)
        {
            ss = score;
            Destroy(currentLevel);
            mainMenu.SetActive(true);
        }
    }
}