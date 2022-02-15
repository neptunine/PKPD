using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    public class GameController : MonoBehaviour
    {
        [SerializeField]
        LevelController
            level;

        [SerializeField]
        LifeHandler
            life;

        [SerializeField]
        GameObject
            menuObject,
            mainMenu,
            levelMenu,
            optionsMenu,
            creditsMenu,
            levelUI;

        public TextAsset[]
            wordFile;

        public int
            ss;

        private void Awake()
        {
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

        public void StartLevel(int mode)
        {
            if (life.isOutOfLife)
            {
                Debug.Log($"[<color=magenta>GameController</color>] Can't Start Level without life");
                return;
            }
            
            menuObject.SetActive(false);
            level.gameObject.SetActive(true);
            levelUI.SetActive(true);
            string[] words = wordFile[mode].text.Split("\n"[0]);
            for (int i = 0; i < words.Length; i++)
            {
                string tmp = words[i];
                int r = Random.Range(i, words.Length);
                words[i] = words[r];
                words[r] = tmp;
            }
            level.Initialize(words);

            Debug.Log($"[<color=magenta>GameController</color>] Started Level with mode {mode}");
        }

        public void TerminateLevel(int score)
        {
            ss = score;
            level.gameObject.SetActive(false);
            levelUI.SetActive(false);
            menuObject.SetActive(true);

            Debug.Log($"[<color=magenta>GameController</color>] Ended Level");
        }

    }
}