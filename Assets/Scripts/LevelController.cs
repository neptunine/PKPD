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

        public float
            lerpSpeed = 4f;

        [SerializeField]
        GameObject
            charDisplay,
            charInput,
            selector;

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

        public int[]
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
            for (int i = 0; i < abc.Length; i++)
            {
                char tmp = abc[i];
                int r = Random.Range(i, abc.Length);
                abc[i] = abc[r];
                abc[r] = tmp;
            }
        }

        private void Start()
        {
            Initialize();
        }

        private void Update()
        {
            if (selected)
            {
                selector.transform.position = new Vector3(Mathf.Lerp(selector.transform.position.x, selected.transform.position.x, Time.deltaTime * lerpSpeed), Mathf.Lerp(selector.transform.position.y, selected.transform.position.y, Time.deltaTime * lerpSpeed), selector.transform.position.z);
            }
            if (selectedInput)
            {
                selectedInput.position = new Vector3(Mathf.Lerp(selectedInput.position.x, targetPos.x, Time.deltaTime * lerpSpeed), Mathf.Lerp(selectedInput.position.y, targetPos.y, Time.deltaTime * lerpSpeed), selectedInput.position.z);
            }

        }

        public void Initialize()
        {
            targetWord = words[Random.Range(0, words.Length)].Trim().ToUpper();
            Debug.Log($"[{this.name}] picked '{targetWord}'");

            char[] wordChar = targetWord.ToCharArray();
            int len = Mathf.RoundToInt(wordChar.Length * percent);
            missing = new int[len];
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
            controller.TerminateGame(score);
        }

        public void SelectChar(Button clicked)
        {
            selected = clicked.gameObject;

        }

        public void InputChar(Button clicked)
        {
            selectedInput = clicked.transform;
            initalPos = selectedInput.position;
            targetPos = selector.transform.position;

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
                        StartCoroutine(IncorrectInput());
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
                Transform found = charDisplay.transform.parent.Find(no.ToString());
                if (found != null)
                    SelectChar(found.GetComponent<Button>());
            }
            else
            {
                Debug.Log("no more");
            }
        }

        private IEnumerator IncorrectInput()
        {
            yield return new WaitForSeconds(1);

            targetPos = initalPos;
        }
    }
}