using System.Collections;
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
        LifeHandler
            life;

        [SerializeField]
        LevelGraphicsHandler
            graphics;

        [SerializeField]
        ProgressBar
            progress;   

        [SerializeField]
        GameObject
            charPrefab,
            levelUI,
            selector,
            charDisplay,
            nextUI;

        [SerializeField]
        LayoutGroup
            displayLayout,
            inputLayout;

        [SerializeField]
        Text text;

        [System.Serializable]
        public class ABC
        {
            public Sprite
                blank,
                a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z;
        }

        [SerializeField]
        ABC
            characters;

        private string[]
            _wordList;

        public int
            quest,
            stage;

        public float
            percent;

        private string
            _targetWord;

        private char[]
            _expectChars,
            _inputChars;

        private Button[]
            _wordButtons,
            _inputButtons;

        private int
            _selectedChar,
            _selectedInput;

        private Vector3
            _initalPos,
            _targetPos;

        public bool
            isQuestEnded,
            isLevelEnded;

        private bool[]
            _results;

        private int
            _inputs = 18;

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

        public void Initialize(string[] words)
        {
            _wordList = words;
            for (int i = 0; i < _wordList.Length; i++)
            {
                _wordList[i] = _wordList[i].Trim().ToUpper();
            }
            quest = 0;
            _results = new bool[_wordList.Length];
            isQuestEnded = isLevelEnded = false;
            Clear();
            NewWord();
        }

        public void NewWord()
        {
            nextUI.SetActive(false);
            isQuestEnded = false;
            quest += 1;
            if (isLevelEnded)
            {
                StartCoroutine(EndScreen());
                return;
            }

            _targetWord = _wordList[quest - 1];
            Debug.Log($"[<color=cyan>LevelController</color>] Target word is '{_targetWord}'");
            char[] abc = "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

            char[] disChars = _targetWord.ToCharArray();
            char[] inChars = new char[_inputs];
            _expectChars = new char[disChars.Length];
            _wordButtons = new Button[disChars.Length];
            _inputButtons = new Button[_inputs];
            percent = Mathf.Clamp01(percent);
            int len = Mathf.RoundToInt(disChars.Length * percent);
            if (len < disChars.Length)
            {
                for (int i = 0; i < len;)
                {
                    int index = Random.Range(0, disChars.Length - 1);
                    if (disChars[index] != '\0')
                    {
                        inChars[index] = _expectChars[index] = disChars[index];
                        disChars[index] = '\0';
                        i++;
                    }
                }
            }
            else
            {
                for (int i = 0; i < len; i++)
                {
                    inChars[i] = _expectChars[i] = disChars[i];
                    disChars[i] = '\0';
                }
            }

            for (int i = 0; i < disChars.Length; i++)
            {
                GameObject charObject = Instantiate(charPrefab.gameObject, charDisplay.transform);
                //charObject.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
                charObject.name = "W." + i.ToString();
                charObject.GetComponentInChildren<Image>().sprite = GetSprite(disChars[i]);
                //CharacterHandler character = charObject.GetComponentInChildren<CharacterHandler>();
                //character.index = i;
                //character.character = disChars[i];
                Button button = charObject.GetComponentInChildren<Button>();
                _wordButtons[i] = button;
                SmoothFollow sf = charObject.GetComponent<SmoothFollow>();
                sf.anchor.transform.SetParent(displayLayout.transform);
                sf.anchor.name = "w." + i.ToString();
                if (disChars[i] == '\0')
                {
                    charObject.name += "_";
                    button.interactable = true;
                    int self = i;
                    button.onClick.AddListener(() => SelectChar(self));
                    if (_selectedChar == -1)
                        SelectChar(i);
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

            _inputChars = inChars;
            for (int i = 0; i < inChars.Length; i++)
            {
                GameObject charObject = Instantiate(charPrefab.gameObject, charDisplay.transform);
                //charObject.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
                charObject.name = "D." + i.ToString();
                charObject.GetComponentInChildren<Image>().sprite = GetSprite(inChars[i]);
                //CharacterHandler character = charObject.GetComponentInChildren<CharacterHandler>();
                //character.index = i;
                //character.character = inChars[i];
                SmoothFollow sf = charObject.GetComponent<SmoothFollow>();
                sf.anchor.transform.SetParent(inputLayout.transform);
                sf.anchor.name = "d." + i.ToString();
                Button button = charObject.GetComponentInChildren<Button>();
                _inputButtons[i] = button;
                button.interactable = true;
                int self = i;
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
            _selectedChar = _selectedInput = -1;
            _targetWord = null;
            _expectChars = null;
            text.text = string.Empty;
            graphics.Clear();
            stage = graphics.Jar.Length - 1;

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

        private Sprite GetSprite(char character)
        {
            switch (character)
            {
                case 'a': case 'A': return characters.a;
                case 'b': case 'B': return characters.b;
                case 'c': case 'C': return characters.c;
                case 'd': case 'D': return characters.d;
                case 'e': case 'E': return characters.e;
                case 'f': case 'F': return characters.f;
                case 'g': case 'G': return characters.g;
                case 'h': case 'H': return characters.h;
                case 'i': case 'I': return characters.i;
                case 'j': case 'J': return characters.j;
                case 'k': case 'K': return characters.k;
                case 'l': case 'L': return characters.l;
                case 'm': case 'M': return characters.m;
                case 'n': case 'N': return characters.n;
                case 'o': case 'O': return characters.o;
                case 'p': case 'P': return characters.p;
                case 'q': case 'Q': return characters.q;
                case 'r': case 'R': return characters.r;
                case 's': case 'S': return characters.s;
                case 't': case 'T': return characters.t;
                case 'u': case 'U': return characters.u;
                case 'v': case 'V': return characters.v;
                case 'w': case 'W': return characters.w;
                case 'x': case 'X': return characters.x;
                case 'y': case 'Y': return characters.y;
                case 'z': case 'Z': return characters.z;
                default: return characters.blank;
            }
        }

        public void SelectChar(int clicked)
        {
            _selectedChar = clicked;
            selector.GetComponent<SmoothFollow>().anchor = _wordButtons[clicked].transform;
        }

        public void InputChar(int clicked)
        {
            if (isQuestEnded || isLevelEnded)
                return;

            if (_selectedInput != -1)
            {
                _inputButtons[_selectedInput].GetComponent<SmoothFollow>().anchor.position = _initalPos;
                _inputButtons[_selectedInput].interactable = true;
            }

            _selectedInput = clicked;
            SmoothFollow sf = _inputButtons[_selectedInput].GetComponent<SmoothFollow>();
            _initalPos = sf.anchor.position;
            sf.anchor.position = selector.GetComponent<SmoothFollow>().anchor.position;
            _inputButtons[_selectedInput].interactable = false;

            char inchar = _inputChars[clicked];

            if (inchar != _expectChars[_selectedChar])
            {
                stage -= 1;
                if (stage < 1)
                    isQuestEnded = true;
                StartCoroutine(OnIncorrectInput());
                return;
            }
            _expectChars[_selectedChar] = '\0';

            int next = -1;
            for (int j = _selectedChar; j < _expectChars.Length; j++)
            {
                if (_expectChars[j] != '\0')
                {
                    next = j;
                    break;
                }
            }
            if (next == -1)
            {
                for (int j = 0; j < _selectedChar; j++)
                {
                    if (_expectChars[j] != '\0')
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
                isQuestEnded = true;
                StartCoroutine(OnEnd());
            }
            _selectedInput = -1;
        }

        private IEnumerator OnCorrectInput(int next)
        {
            SmoothFollow sf = _inputButtons[_selectedInput].GetComponent<SmoothFollow>();
            sf.anchor = _wordButtons[_selectedChar].transform;
            _inputButtons[_selectedInput].interactable = false;
            _selectedChar = next;

            float t;
            if (sf.mode == SmoothFollow.modeSetting.Lerp)
                t = 1 / sf.lerpSpeed;
            else
                t = sf.smoothTime;
            yield return new WaitForSeconds(t);

            selector.GetComponent<SmoothFollow>().anchor = _wordButtons[_selectedChar].transform;
        }

        private IEnumerator OnIncorrectInput()
        {
            SmoothFollow sf = _inputButtons[_selectedInput].GetComponent<SmoothFollow>();
            float t;
            if (sf.mode == SmoothFollow.modeSetting.Lerp)
                t = 1 / sf.lerpSpeed * 2;
            else
                t = sf.smoothTime * 2;
            yield return new WaitForSeconds(t);

            if (_selectedInput != -1)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(sf.anchor.transform.parent.GetComponent<RectTransform>());
                _inputButtons[_selectedInput].interactable = true;
            }

            graphics.NextStage();
            if (graphics.isFinal)
                StartCoroutine(OnGameOver());

            _selectedInput = -1;
        }

        private IEnumerator OnEnd()
        {
            Debug.Log($"[<color=cyan>LevelController</color>] Question finish");

            _results[quest - 1] = true;
            QuestionEnd();

            foreach (Button button in charDisplay.GetComponentsInChildren<Button>())
                button.interactable = false;

            yield return new WaitForSeconds(2);
            text.text = "Finish";

            nextUI.SetActive(true);
        }

        private IEnumerator OnGameOver()
        {
            Debug.Log($"[<color=cyan>LevelController</color>] Question Failed");

            life.Damage();
            _results[quest - 1] = false;
            QuestionEnd();

            foreach (Button button in charDisplay.GetComponentsInChildren<Button>())
                button.interactable = false;

            yield return new WaitForSeconds(2);
            text.text = "Failed";

            nextUI.SetActive(true);
        }

        private IEnumerator EndScreen()
        {
            Debug.Log($"[<color=cyan>LevelController</color>] Level Ended (score: {score}%)");

            yield return new WaitForSeconds(1);
            text.text = "Gameover";
            yield return new WaitForSeconds(2);
            text.text = "Score: " + score;
        }

        private void QuestionEnd()
        {
            progress.value = 1f * quest / _wordList.Length;
            if (quest > _wordList.Length || life.isOutOfLife)
                isLevelEnded = true;

            float s = 0f;
            for (int i = 0; i < _results.Length; i++)
            {
                if (_results[i])
                    s += 100f;
            }
            s /= _results.Length;
            score = Mathf.RoundToInt(s);
        }
    }
}