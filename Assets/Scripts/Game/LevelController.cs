using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Utility;
using Journal;

namespace Game
{
    public class LevelController : MonoBehaviour
    {
        private GameController
            _controller;

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
            nextUI,
            endScreenUI;

        [SerializeField]
        private LayoutGroup
            displayLayout,
            inputLayout;

        public RectTransform
            displaySpawn,
            inputSpawn;

        [SerializeField]
        private TMP_Text
            levelText;

        public string
            passText = "Finish",
            failText = "Failed";

        [System.Serializable]
        public class JournalID
        {
            public int[]
                correctInput,
                inorrectInput,
                wordPass,
                wordFail,
                levelComplete,
                levelFail,
                exp;
        }

        public JournalID
            journal;

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

        private int
            _quest,
            _stage;

        private float
            _hidePercent;

        private bool
            _autoSelect;

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
            _isLevelEnded,
            _isdead;

        private bool[]
            _results;

        private float
            _expScale;

        private readonly int
            _inputs = 24;

        public void SetController(GameController controller)
        {
            _controller = controller;
        }

        private void Awake()
        {

        }

        private void Start()
        {

        }

        private void Update()
        {

        }

        public void Initialize(string[] words, float hide, bool select, float expScale = 1f)
        {
            _wordList = words;
            _hidePercent = hide;
            _autoSelect = select;
            _expScale = expScale;
            for (int i = 0; i < _wordList.Length; i++)
            {
                _wordList[i] = _wordList[i].Trim().ToUpper();
            }
            _quest = 0;
            _results = new bool[_wordList.Length];
            _isQuestEnded = _isLevelEnded = _isdead = false;
            selector.SetActive(!_autoSelect);
            endScreenUI.SetActive(false);
            Clear();
            NewWord();

            _controller.audioController.PlayLevelStart();
        }

        public void NewWord()
        {
            nextUI.SetActive(false);
            if (_isLevelEnded || _quest >= _wordList.Length)
            {
                StartCoroutine(OnEndScreen(_isdead));
                return;
            }
            _isQuestEnded = false;
            _quest += 1;

            _targetWord = _wordList[_quest - 1];
            char[] abc = "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

            char[] disChars = _targetWord.ToCharArray();
            char[] inChars = new char[_inputs];
            _expectChars = new char[disChars.Length];
            _wordButtons = new Button[disChars.Length];
            _inputButtons = new Button[_inputs];
            _hidePercent = Mathf.Clamp01(_hidePercent);
            int len = Mathf.RoundToInt(disChars.Length * _hidePercent);
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
                for (int i = 0; i < disChars.Length; i++)
                {
                    inChars[i] = _expectChars[i] = disChars[i];
                    disChars[i] = '\0';
                }
            }

            for (int i = 0; i < disChars.Length; i++)
            {
                GameObject charObject = Instantiate(charPrefab, displaySpawn.position, Quaternion.identity, charDisplay.transform);
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
                GameObject charObject = Instantiate(charPrefab, inputSpawn.position, Quaternion.identity, charDisplay.transform);
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

            Debug.Log($"[<color=cyan>LevelController</color>] Question {_quest}/{_wordList.Length} Target word is \"{_targetWord}\"; Hide {_hidePercent * 100}%; Auto target {(_autoSelect ? "yes" : "no")}");
        }

        public void Terminate()
        {
            Clear();
            _controller.TerminateLevel();
        }

        public void Clear()
        {
            endScreenUI.SetActive(false);
            _selectedChar = _selectedInput = -1;
            _targetWord = null;
            _expectChars = null;
            levelText.text = string.Empty;
            graphics.Clear();
            _stage = graphics.maxstage;

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
                case '\0': spriteTarget = spriteAlt = characters.blankAlt; break;
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

            if (_autoSelect)
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
                _stage -= 1;
                StartCoroutine(OnIncorrectInput());
                if (_stage < 1)
                {
                    Debug.Log($"[<color=cyan>LevelController</color>] Question Failed");
                    _isQuestEnded = true;
                    _controller.playerData.Damage();
                    _results[_quest - 1] = false;
                    StartCoroutine(OnWordFail());
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
                _results[_quest - 1] = true;
                StartCoroutine(OnWordPass());
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

            if (Random.value < .05f) _controller.audioController.PlayCharacterCorrect();
            _controller.playerData.Increment("CorrectInput");
            foreach (int i in journal.correctInput)
                Journal.Journal.Increment(i, 1);

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

            graphics.SetStage(_stage);
            _selectedInput = -1;

            if (Random.value < .05f) _controller.audioController.PlayCharacterIncorrect();
            _controller.playerData.Increment("IncorrectInput");
            foreach (int i in journal.inorrectInput)
                Journal.Journal.Increment(i, 1);

        }

        private IEnumerator OnWordPass()
        {
            progress.value = 1f * _quest / _wordList.Length;
            if (_quest > _wordList.Length)
                _isLevelEnded = true;
            if (_controller.playerData.isOutOfLife)
                _isLevelEnded = _isdead = true;

            foreach (Button button in charDisplay.GetComponentsInChildren<Button>())
                button.interactable = false;

            _controller.audioController.PlayWordCorrect();
            _controller.playerData.Increment("WordCleared");
            foreach (int i in journal.wordPass)
                Journal.Journal.Increment(i, 1);

            yield return new WaitForSeconds(2);
            levelText.text = passText;
            nextUI.SetActive(true);

        }

        private IEnumerator OnWordFail()
        {
            progress.value = 1f * _quest / _wordList.Length;
            if (_quest > _wordList.Length)
                _isLevelEnded = true;
            if (_controller.playerData.isOutOfLife)
                _isLevelEnded = _isdead = true;

            foreach (Button button in charDisplay.GetComponentsInChildren<Button>())
                button.interactable = false;

            _controller.audioController.PlayWordFail();
            _controller.playerData.Increment("WordFailed");
            foreach (int i in journal.wordFail)
                Journal.Journal.Increment(i, 1);

            yield return new WaitForSeconds(2);
            levelText.text = failText;
            nextUI.SetActive(true);

        }

        private IEnumerator OnEndScreen(bool dead)
        {
            float score = 0f;
            int exp = 0;
            int combo = 1;
            string debug = string.Empty;
            for (int i = 0; i < _results.Length; i++)
            {
                if (_results[i])
                {
                    score += 100f;
                    exp += 10 * combo;
                    debug += $"<color=green>{exp}({combo})</color>; ";
                    if (combo < 8) combo *= 2;
                }
                else
                {
                    debug += $"<color=maroon>{exp}({combo})</color>; ";
                    combo = Mathf.CeilToInt(combo * .5f);
                }
                
            }
            score /= _results.Length;
            exp = Mathf.FloorToInt(exp * _expScale);
            _controller.playerData.AddExperience(exp);
            foreach (int i in journal.exp)
                Journal.Journal.Increment(i, exp);

            Debug.Log($"[<color=cyan>LevelController</color>] Level Ended (score: {score:f2}%; exp: {exp} [{_expScale}x])({debug})");

            graphics.SetStage(-1);
            if (dead)
            {
                _controller.audioController.PlayFailed();
                _controller.playerData.Increment("LevelFailed");
                foreach (int i in journal.levelFail)
                    Journal.Journal.Increment(i, 1);
            }
            else
            {
                _controller.audioController.PlayVictory();
                _controller.playerData.Increment("LevelCleared");
                foreach (int i in journal.levelComplete)
                    Journal.Journal.Increment(i, 1);
            }

            TMP_Text endText = endScreenUI.GetComponentInChildren<TMP_Text>();
            GameObject backButton = endScreenUI.GetComponentInChildren<Button>().gameObject;
            endScreenUI.SetActive(true);
            backButton.SetActive(false);

            string title =
                $"{(dead ? "Game Over" : "Victory")}";

            string text =
                $"\nScore: {Mathf.RoundToInt(score)}" +
                $"\nProgress: {(dead ? _quest - 1 : _quest)}/{_results.Length}" +
                $"\nEXP: {exp:N0}";

            endText.text = "<size=20>";
            foreach (char c in title)
            {
                endText.text += c;
                yield return new WaitForSeconds(.2f);
            }
            endText.text += "</size>";
            
            yield return new WaitForSeconds(1);

            foreach (char c in text)
            {
                endText.text += c;
                yield return new WaitForSeconds(.1f);
            }

            yield return new WaitForSeconds(2);
            backButton.SetActive(true);

        }

    }
}