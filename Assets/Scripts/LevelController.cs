﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace Game
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField]
        GameController
            controller;

        [SerializeField]
        LevelGraphicsHandler
            graphics;

        [SerializeField]
        CharacterHandler
            charPrefab;

        [SerializeField]
        GameObject
            levelUI,
            selector,
            charDisplay;

        [SerializeField]
        LayoutGroup
            displayLayout,
            inputLayout;

        [SerializeField]
        Text text;

        [Header("Settings")]

        public string[]
            wordList;

        public int
            mode;

        public float
            percent;

        private string
            targetWord;

        private char[]
           expectChar;

        private Transform
            selected,
            selectedInput;

        private Vector3
            initalPos,
            targetPos;

        public int
            score;

        private void Awake()
        {

        }

        private void Start()
        {

        }

        private void Update()
        {

        }

        public void Initialize()
        {
            Clear();

            targetWord = wordList[Random.Range(0, wordList.Length)].Trim().ToUpper();
            Debug.Log($"[{this.name}] Target word is '{targetWord}'");
            char[] abc = "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

            char[] disChars = targetWord.ToCharArray();
            char[] inChars = new char[18];
            expectChar = new char[disChars.Length];
            percent = Mathf.Clamp01(percent);
            int len = Mathf.RoundToInt(disChars.Length * percent);
            if (len < disChars.Length)
            {
                for (int i = 0; i < len;)
                {
                    int index = Random.Range(0, disChars.Length - 1);
                    if (disChars[index] != '\0')
                    {
                        inChars[index] = expectChar[index] = disChars[index];
                        disChars[index] = '\0';
                        i++;
                    }
                }
            }
            else
            {
                for (int i = 0; i < len; i++)
                {
                    inChars[i] = expectChar[i] = disChars[i];
                    disChars[i] = '\0';
                }
            }

            for (int i = 0; i < disChars.Length; i++)
            {
                GameObject charObject = Instantiate(charPrefab.gameObject, charDisplay.transform);
                //charObject.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
                charObject.name = '$' + i.ToString();
                CharacterHandler character = charObject.GetComponentInChildren<CharacterHandler>();
                character.index = i;
                character.character = disChars[i];
                SmoothFollow sf = charObject.GetComponent<SmoothFollow>();
                sf.anchor.transform.SetParent(displayLayout.transform);
                sf.anchor.name = '$' + i.ToString();
                if (disChars[i] == '\0')
                {
                    Button button = charObject.GetComponentInChildren<Button>();
                    if (selected == null)
                        SelectChar(button);
                    button.interactable = true;
                    Button self = button;
                    button.onClick.AddListener(() => SelectChar(self));
                }
                charObject.SetActive(true);
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(displayLayout.GetComponent<RectTransform>());

            for (int i = 0; i < abc.Length; i++)
            {
                char tmp = abc[i];
                int r = Random.Range(i, abc.Length);
                abc[i] = abc[r];
                abc[r] = tmp;
            }
            for (int i = 0; i < inChars.Length; i++)
            {
                if (inChars[i] == 0)
                {
                    inChars[i] = abc[i];
                }
            }
            for (int i = 0; i < inChars.Length; i++)
            {
                char tmp = inChars[i];
                int r = Random.Range(i, inChars.Length);
                inChars[i] = inChars[r];
                inChars[r] = tmp;
            }

            for (int i = 0; i < inChars.Length; i++)
            {
                GameObject charObject = Instantiate(charPrefab.gameObject, charDisplay.transform);
                //charObject.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
                charObject.name = '_' + i.ToString();
                CharacterHandler character = charObject.GetComponentInChildren<CharacterHandler>();
                character.index = i;
                character.character = inChars[i];
                SmoothFollow sf = charObject.GetComponent<SmoothFollow>();
                sf.anchor.transform.SetParent(inputLayout.transform);
                sf.anchor.name = '_' + i.ToString();
                Button button = charObject.GetComponentInChildren<Button>();
                button.interactable = true;
                Button self = button;
                button.onClick.AddListener(() => InputChar(self));
                charObject.SetActive(true);
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(inputLayout.GetComponent<RectTransform>());

        }

        public void Terminate()
        {
            Clear();
            controller.TerminateLevel(score);
        }

        public void Clear()
        {
            selected = selectedInput = null;
            targetWord = null;
            expectChar = null;

            for (int i = 0; i < charDisplay.transform.childCount; i++)
            {
                Destroy(charDisplay.transform.GetChild(i).gameObject);
            }
            for (int i = 0; i < displayLayout.transform.childCount; i++)
            {
                Destroy(displayLayout.transform.GetChild(i).gameObject);
            }
            for (int i = 0; i < inputLayout.transform.childCount; i++)
            {
                Destroy(inputLayout.transform.GetChild(i).gameObject);
            }
        }

        public void SelectChar(Button clicked)
        {
            selected = clicked.transform;
            selector.GetComponent<SmoothFollow>().anchor = selected;
        }

        public void InputChar(Button clicked)
        {
            if (selectedInput)
            {
                selectedInput.GetComponent<SmoothFollow>().anchor.position = initalPos;
                selectedInput.GetComponent<Button>().interactable = true;
            }

            selectedInput = clicked.transform;
            initalPos = selectedInput.position;
            selectedInput.GetComponent<SmoothFollow>().anchor.position = selector.GetComponent<SmoothFollow>().anchor.position;
            selectedInput.GetComponent<Button>().interactable = false;

            char inchar = selectedInput.GetComponentInChildren<CharacterHandler>().character;
            int i = selected.GetComponentInChildren<CharacterHandler>().index;

            if (inchar != expectChar[i])
            {
                StartCoroutine(OnIncorrectInput());
                return;
            }
            expectChar[i] = '\0';

            int next = -1;
            for (int j = i; j < expectChar.Length; j++)
            {
                if (expectChar[j] != '\0')
                {
                    next = j;
                    break;
                }
            }
            if (next == -1)
            {
                for (int j = 0; j < i; j++)
                {
                    if (expectChar[j] != '\0')
                    {
                        next = j;
                        break;
                    }
                }
            }

            if (next != -1)
            {
                StartCoroutine(OnCorrectInput(next));
            }
            else
            {
                StartCoroutine(OnEnd());
            }
            selectedInput = null;
        }

        private IEnumerator OnCorrectInput(int i)
        {
            SmoothFollow sf = selectedInput.GetComponent<SmoothFollow>();
            sf.anchor = selected;
            selected.GetComponent<Button>().interactable = false;
            selected = charDisplay.transform.Find('$' + i.ToString());

            float t;
            if (sf.mode == SmoothFollow.modeSetting.Lerp)
                t = 1 / sf.lerpSpeed;
            else
                t = sf.smoothTime;
            yield return new WaitForSeconds(t);

            selector.GetComponent<SmoothFollow>().anchor = selected;
        }

        private IEnumerator OnIncorrectInput()
        {
            SmoothFollow sf = selectedInput.GetComponent<SmoothFollow>();
            float t;
            if (sf.mode == SmoothFollow.modeSetting.Lerp)
                t = 1 / sf.lerpSpeed * 2;
            else
                t = sf.smoothTime * 2;
            yield return new WaitForSeconds(t);

            if (selectedInput)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(sf.anchor.transform.parent.GetComponent<RectTransform>());
                selectedInput.GetComponent<Button>().interactable = true;
            }

            graphics.nextStage();
            if (graphics.isFinal)
                StartCoroutine(OnGameOver());

            selectedInput = null;
        }

        private IEnumerator OnEnd()
        {
            Debug.Log($"[{this.name}] Finish");

            foreach (Button button in charDisplay.GetComponentsInChildren<Button>())
                button.interactable = false;

            yield return new WaitForSeconds(2);
            text.text = "Finish";
        }

        private IEnumerator OnGameOver()
        {
            Debug.Log($"[{this.name}] Game Over");

            foreach (Button button in charDisplay.GetComponentsInChildren<Button>())
                button.interactable = false;

            yield return new WaitForSeconds(2);
            text.text = "Game Over";
        }
    }
}