using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField]
        GameController
            controller;

        public TextAsset
            wordFile;

        public int
            mode;

        public float
            percent;

        public Color
            initcolor,
            selectColor;

        [SerializeField]
        GameObject
            charDisplay;

        private Transform
            charParent;

        [SerializeField]
        Text text;

        private string[]
            words;

        private char
            hideChar = '_',
            spacChar = ' ';

        private string
            targetWord,
            displayWord;

        private GameObject
            selected;

        public int
            score;

        private void Awake()
        {
            words = wordFile.text.Split("\n"[0]);
        }

        private void Start()
        {
            charParent = charDisplay.transform.parent;
            initcolor = charDisplay.GetComponent<Image>().color;
            Initialize();
        }

        private void Update()
        {

        }

        public void Initialize()
        {
            targetWord = words[Random.Range(0, words.Length)].Trim();
            Debug.Log($"[{this.name}] picked '{targetWord}'");

            char[] wordChar = targetWord.ToCharArray();
            for (int i = 0; i < wordChar.Length * percent;)
            {
                int index = Random.Range(0, wordChar.Length - 1);
                if (wordChar[index] != hideChar)
                {
                    wordChar[index] = hideChar;
                    i++;
                }
            }
            //char[] tmpChar2 = new char[tmpChar.Length * 2 - 1];
            //for (int i = 0; i < tmpChar2.Length; i++)
            //{
            //    if (i % 2 == 0) tmpChar2[i] = tmpChar[i / 2];
            //    else tmpChar2[i] = spacChar;
            //}

            for (int i = 0; i < wordChar.Length; i++)
            {
                GameObject charObject = Instantiate(charDisplay, charDisplay.transform.parent);
                charObject.name = i.ToString();
                charObject.GetComponentInChildren<Text>().text = wordChar[i].ToString();
                if (wordChar[i] == hideChar)
                {
                    Button button = charObject.GetComponentInChildren<Button>();
                    button.interactable = true;
                    Button self = button;
                    button.onClick.AddListener(() => SelectChar(self));
                }
                charObject.SetActive(true);
            }

            displayWord = new string(wordChar);

            text.text = displayWord;
        }

        public void Terminate()
        {
            controller.TerminateGame(score);
        }

        public void SelectChar(Button clicked)
        {
            if (selected != null)
            {
                selected.GetComponent<Image>().color = initcolor;
            }

            selected = clicked.gameObject;
            selected.GetComponent<Image>().color = selectColor;
        }
    }
}