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

        private void Start()
        {

        }

        private void Update()
        {

        }

        public void InitializeGame(int mode)
        {
            mainMenu.SetActive(false);
            currentLevel = Instantiate(level);
            currentLevelController = currentLevel.GetComponent<LevelController>();
            currentLevelController.wordFile = wordFile[mode];
            currentLevel.SetActive(true);
        }
    }
}