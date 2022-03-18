using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace Game
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField]
        private GameController
            controller;

        [SerializeField]
        private LifeHandler
            life;

        [SerializeField]
        private LevelGraphicsHandler
            graphics;

        [SerializeField]
        private ProgressBar
            progress;

        [SerializeField]
        private GameObject
            charPrefab,
            levelUI,
            selector,
            charDisplay,
            nextUI;

        [SerializeField]
        private LayoutGroup
            displayLayout,
            inputLayout;

        [SerializeField]
        private Text text;

        [System.Serializable]
        public class ABC
        {
            public Sprite
                blankTarget, blankAlt,
                aTarget, aAlt,
                bTarget, bAlt,
                cTarget, cAlt,
                dTarget, dAlt,
                eTarget, eAlt,
                fTarget, fAlt,
                gTarget, gAlt,
                hTarget, hAlt,
                iTarget, iAlt,
                jTarget, jAlt,
                kTarget, kAlt,
                lTarget, lAlt,
                mTarget, mAlt,
                nTarget, nAlt,
                oTarget, oAlt,
                pTarget, pAlt,
                qTarget, qAlt,
                rTarget, rAlt,
                sTarget, sAlt,
                tTarget, tAlt,
                uTarget, uAlt,
                vTarget, vAlt,
                wTarget, wAlt,
                xTarget, xAlt,
                yTarget, yAlt,
                zTarget, zAlt;
        }

        [SerializeField]
        private ABC
            characters;

        private string[]
            _wordList;

        public int
            quest,
            stage;

        public float
            percent;

        public bool
            autoSelect;

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

        private bool
            _isQuestEnded,
            _isLevelEnded;

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
            _isQuestEnded = _isLevelEnded = false;
            selector.SetActive(!autoSelect);
            Clear();
            NewWord();
        }

        public void NewWord()
        {
            nextUI.SetActive(false);
            _isQuestEnded = false;
            quest += 1;
            if (_isLevelEnded)
            {
                StartCoroutine(EndScreen());
                return;
            }

            _targetWord = _wordList[quest - 1];
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
                //charObject.GetComponentInChildren<Image>().sprite = GetSprite(disChars[i]);
                //CharacterHandler character = charObject.GetComponentInChildren<CharacterHandler>();
                //character.index = i;
                //character.character = disChars[i];
                Button button = charObject.GetComponentInChildren<Button>();
                SetButtonSprite(button, disChars[i]);
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
                //charObject.GetComponentInChildren<Image>().sprite = GetSprite(inChars[i]);
                //CharacterHandler character = charObject.GetComponentInChildren<CharacterHandler>();
                //character.index = i;
                //character.character = inChars[i];
                SmoothFollow sf = charObject.GetComponent<SmoothFollow>();
                sf.anchor.transform.SetParent(inputLayout.transform);
                sf.anchor.name = "d." + i.ToString();
                Button button = charObject.GetComponentInChildren<Button>();
                SetButtonSprite(button, inChars[i]);
                _inputButtons[i] = button;
                button.interactable = true;
                int self = i;
                button.onClick.AddListener(() => InputChar(self));
                charObject.SetActive(true);
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(inputLayout.GetComponent<RectTransform>());

            Debug.Log($"[<color=cyan>LevelController</color>] Target word is '{_targetWord}'; Hide {percent * 100}%; Auto target {(autoSelect ? "yes" : "no")}");
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
            stage = graphics.maxstage;

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

        private void SetButtonSprite(Button button, char character)
        {
            Sprite spriteTarget, spriteAlt;

            switch (character)
            {
                case 'a': case 'A': spriteTarget = characters.aTarget; spriteAlt = characters.aAlt; break;
                case 'b': case 'B': spriteTarget = characters.bTarget; spriteAlt = characters.bAlt; break;
                case 'c': case 'C': spriteTarget = characters.cTarget; spriteAlt = characters.cAlt; break;
                case 'd': case 'D': spriteTarget = characters.dTarget; spriteAlt = characters.dAlt; break;
                case 'e': case 'E': spriteTarget = characters.eTarget; spriteAlt = characters.eAlt; break;
                case 'f': case 'F': spriteTarget = characters.fTarget; spriteAlt = characters.fAlt; break;
                case 'g': case 'G': spriteTarget = characters.gTarget; spriteAlt = characters.gAlt; break;
                case 'h': case 'H': spriteTarget = characters.hTarget; spriteAlt = characters.hAlt; break;
                case 'i': case 'I': spriteTarget = characters.iTarget; spriteAlt = characters.iAlt; break;
                case 'j': case 'J': spriteTarget = characters.jTarget; spriteAlt = characters.jAlt; break;
                case 'k': case 'K': spriteTarget = characters.kTarget; spriteAlt = characters.kAlt; break;
                case 'l': case 'L': spriteTarget = characters.lTarget; spriteAlt = characters.lAlt; break;
                case 'm': case 'M': spriteTarget = characters.mTarget; spriteAlt = characters.mAlt; break;
                case 'n': case 'N': spriteTarget = characters.nTarget; spriteAlt = characters.nAlt; break;
                case 'o': case 'O': spriteTarget = characters.oTarget; spriteAlt = characters.oAlt; break;
                case 'p': case 'P': spriteTarget = characters.pTarget; spriteAlt = characters.pAlt; break;
                case 'q': case 'Q': spriteTarget = characters.qTarget; spriteAlt = characters.qAlt; break;
                case 'r': case 'R': spriteTarget = characters.rTarget; spriteAlt = characters.rAlt; break;
                case 's': case 'S': spriteTarget = characters.sTarget; spriteAlt = characters.sAlt; break;
                case 't': case 'T': spriteTarget = characters.tTarget; spriteAlt = characters.tAlt; break;
                case 'u': case 'U': spriteTarget = characters.uTarget; spriteAlt = characters.uAlt; break;
                case 'v': case 'V': spriteTarget = characters.vTarget; spriteAlt = characters.vAlt; break;
                case 'w': case 'W': spriteTarget = characters.wTarget; spriteAlt = characters.wAlt; break;
                case 'x': case 'X': spriteTarget = characters.xTarget; spriteAlt = characters.xAlt; break;
                case 'y': case 'Y': spriteTarget = characters.yTarget; spriteAlt = characters.yAlt; break;
                case 'z': case 'Z': spriteTarget = characters.zTarget; spriteAlt = characters.zAlt; break;
                default: spriteTarget = characters.blankTarget; spriteAlt = characters.blankAlt; break;
            }
            button.GetComponentInChildren<Image>().sprite = spriteTarget;
            SpriteState spriteState = new SpriteState
            {
                highlightedSprite = spriteTarget,
                pressedSprite = spriteAlt,
                selectedSprite = spriteAlt,
                disabledSprite = spriteTarget
            };
            button.spriteState = spriteState;
        }

        public void SelectChar(int clicked)
        {
            _selectedChar = clicked;
            selector.GetComponent<SmoothFollow>().anchor = _wordButtons[clicked].transform;
        }

        public void InputChar(int clicked)
        {
            if (_isQuestEnded || _isLevelEnded)
                return;

            if (_selectedInput != -1)
            {
                _inputButtons[_selectedInput].GetComponent<SmoothFollow>().anchor.position = _initalPos;
                _inputButtons[_selectedInput].interactable = true;
            }

            if (autoSelect)
            {
                int index = System.Array.IndexOf(_expectChars, _inputChars[clicked]);
                if (index != -1)
                    SelectChar(index);
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
                StartCoroutine(OnIncorrectInput());
                if (stage < 1)
                {
                    Debug.Log($"[<color=cyan>LevelController</color>] Question Failed");
                    _isQuestEnded = true;
                    life.Damage();
                    _results[quest - 1] = false;
                    StartCoroutine(OnGameOver());
                }
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
                Debug.Log($"[<color=cyan>LevelController</color>] Question finish");
                _isQuestEnded = true;
                _results[quest - 1] = true;
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

            graphics.SetStage(stage);

            _selectedInput = -1;
        }

        private IEnumerator OnEnd()
        {
            QuestionEnd();

            foreach (Button button in charDisplay.GetComponentsInChildren<Button>())
                button.interactable = false;

            yield return new WaitForSeconds(2);
            text.text = "Finish";

            nextUI.SetActive(true);
        }

        private IEnumerator OnGameOver()
        {
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
                _isLevelEnded = true;

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