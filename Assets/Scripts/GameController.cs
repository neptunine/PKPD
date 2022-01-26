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
            creditsMenu;

        [SerializeField]
        LevelController
            level;

        public TextAsset[]
            wordFile;

        private LevelController
            currentLevel;

        public int
            ss;

        private void Awake()
        {
            level.gameObject.SetActive(false);
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

        public void StartLevel(int mode)
        {
            menuObject.SetActive(false);
            currentLevel = Instantiate(level.gameObject).GetComponent<LevelController>();
            currentLevel.wordFile = wordFile[mode];
            currentLevel.Initialize();
            currentLevel.gameObject.SetActive(true);
        }

        public void TerminateLevel(int score)
        {
            ss = score;
            Destroy(currentLevel.gameObject);
            menuObject.SetActive(true);
        }

    }
}