using System.Collections;
using System.Collections.Generic;
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
        GameObject
            charDisplay,
            charInput,
            selector;

        [SerializeField]
        Text text;

        [Header("Settings")]

        public TextAsset
            wordFile;

        public int
            mode;

        public float
            percent;

        private Transform
            selectedInput;

        private Vector3
            initalPos,
            targetPos;

        private string[]
            words;

        private char
            hideChar = ' ';

        private char[]
            inputChars = new char[18];

        private string
            targetWord;

        private int[]
            missing;

        private GameObject
            selected;

        public int
            score;

        private char[]
            abc;

        private void Awake()
        {
            words = wordFile.text.Split("\n"[0]);
            abc = "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        }

        private void Start()
        {
            Initialize();
        }

        private void Update()
        {

        }

        public void Initialize()
        {
            targetWord = words[Random.Range(0, words.Length)].Trim().ToUpper();
            Debug.Log($"[{this.name}] picked '{targetWord}'");

            char[] wordChar = targetWord.ToCharArray();
            percent = Mathf.Clamp01(percent);
            int len = Mathf.RoundToInt(wordChar.Length * percent);
            missing = new int[len];
            if (len < wordChar.Length)
            {
                for (int i = 0; i < len;)
                {
                    int index = Random.Range(0, wordChar.Length - 1);
                    if (wordChar[index] != hideChar)
                    {
                        inputChars[index] = wordChar[index];
                        wordChar[index] = hideChar;
                        i++;
                    }
                }
            }
            else
            {
                for (int i = 0; i < len; i++)
                {
                    inputChars[i] = wordChar[i];
                    wordChar[i] = hideChar;
                }
            }
            for (int i = 0, j = 0; i < wordChar.Length; i++)
            {
                if (wordChar[i] == hideChar)
                {
                    missing[j] = i;
                    j++;
                }
            }

            for (int i = 0; i < wordChar.Length; i++)
            {
                GameObject charObject = Instantiate(charDisplay, charDisplay.transform.parent);
                charObject.name = i.ToString();
                charObject.GetComponentInChildren<Text>().text = wordChar[i].ToString();
                if (wordChar[i] == hideChar)
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

            for (int i = 0; i < abc.Length; i++)
            {
                char tmp = abc[i];
                int r = Random.Range(i, abc.Length);
                abc[i] = abc[r];
                abc[r] = tmp;
            }
            for (int i = 0; i < inputChars.Length; i++)
            {
                if (inputChars[i] == 0)
                {
                    inputChars[i] = abc[i];
                }
            }
            for (int i = 0; i < inputChars.Length; i++)
            {
                char tmp = inputChars[i];
                int r = Random.Range(i, inputChars.Length);
                inputChars[i] = inputChars[r];
                inputChars[r] = tmp;
            }

            for (int i = 0; i < inputChars.Length; i++)
            {
                GameObject charObject = Instantiate(charInput, charInput.transform.parent);
                charObject.name = i.ToString();
                charObject.GetComponentInChildren<Text>().text = inputChars[i].ToString();
                Button button = charObject.GetComponentInChildren<Button>();
                button.interactable = true;
                Button self = button;
                button.onClick.AddListener(() => InputChar(self));
                charObject.SetActive(true);
            }

        }

        public void Terminate()
        {
            controller.TerminateLevel(score);
        }

        public void SelectChar(Button clicked)
        {
            selected = clicked.gameObject;
            selector.GetComponent<SmoothFollow>().anchor = selected.transform;

        }

        public void InputChar(Button clicked)
        {
            if (selectedInput)
                selectedInput.GetComponent<SmoothFollow>().anchor.position = initalPos;

            selectedInput = clicked.transform;
            initalPos = selectedInput.position;
            selectedInput.GetComponent<SmoothFollow>().anchor.position = selector.transform.position;

            char inchar = selectedInput.GetComponentInChildren<Text>().text.ToCharArray()[0];
            int no, k;
            int.TryParse(selected.name, out no);
            for (int i = 0; i < missing.Length; i++)
            {
                if (missing[i] == no)
                {
                    k = missing[i];

                    if (inchar != targetWord[k])
                    {
                        StartCoroutine(OnIncorrectInput());
                        return;
                    }

                    missing[i] = -1;

                    if (i < missing.Length - 1)
                    {
                        no = missing[i + 1];
                        break;
                    }
                    else
                    {
                        no = -1;
                        for (int j = 0; j < missing.Length; j++)
                        {
                            if (missing[j] != -1) no = missing[j];
                        }
                        break;
                    }
                }
            }

            if (no != -1)
            {
                StartCoroutine(OnCorrectInput(no));
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
            sf.anchor = selected.transform;

            float t;
            if (sf.mode == SmoothFollow.modeSetting.Lerp)
                t = 1 / sf.lerpSpeed;
            else
                t = sf.smoothTime;
            yield return new WaitForSeconds(t);

            SelectChar(charDisplay.transform.parent.Find(i.ToString()).GetComponent<Button>());
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
                LayoutRebuilder.ForceRebuildLayoutImmediate(sf.anchor.transform.parent.GetComponent<RectTransform>());
            //sf.anchor.position = initalPos;

            graphics.nextStage();
            if (graphics.isFinal)
                StartCoroutine(OnGameOver());
        }

        private IEnumerator OnEnd()
        {
            Debug.Log($"[{this.name}] Finish");
            yield return new WaitForSeconds(2);
            text.text = "Finish";
        }

        private IEnumerator OnGameOver()
        {
            Debug.Log($"[{this.name}] Game Over");
            yield return new WaitForSeconds(2);
            text.text = "Game Over";
        }
    }
}