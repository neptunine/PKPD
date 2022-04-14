using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class LevelSelection : MonoBehaviour
    {
        [SerializeField]
        private Transform
            topicButtonsParent,
            difficultyButtonsParent;

        [System.Serializable]
        public class Difficulty
        {
            [Min(1)]
            public Vector2Int
                words = new Vector2Int(10, 20);

            [Range(0, 1)]
            public float
                hidePercentage = 1f;

            public bool
                manualSelection = false;

            [Min(0)]
            public float
                EXPMultiplier = 1f;
        }

        public Difficulty[]
            difficulties;

        private int _topic;
        public int topic { get { return _topic; } }
        private int _difficulty;
        public int difficulty { get { return _difficulty; } }
        private int _wordMin;
        public int wordMin { get { return _wordMin; } }
        private int _wordMax;
        public int wordMax { get { return _wordMax; } }
        private float _hide;
        public float hide { get { return _hide; } }
        private bool _manual;
        public bool manual { get { return _manual; } }
        private float _expMultiplier;
        public float expMultiplier { get { return _expMultiplier; } }

        void Start()
        {
            int i = 1;
            foreach (Transform child in topicButtonsParent)
            {
                Toggle toggle = child.GetComponent<Toggle>();
                if (toggle)
                {
                    if (i == 1) toggle.isOn = true;
                    else toggle.isOn = false;
                    int j = i;
                    Toggle self = toggle;
                    toggle.onValueChanged.AddListener((isOn) => { if (isOn) SelectTopic(j, toggle); });
                    i++;
                }
            }
            i = 0;
            foreach (Transform child in difficultyButtonsParent)
            {
                Toggle toggle = child.GetComponent<Toggle>();
                if (toggle)
                {
                    if (i == 0) toggle.isOn = true;
                    else toggle.isOn = false;
                    int j = i;
                    Toggle self = toggle;
                    toggle.onValueChanged.AddListener((isOn) => { if (isOn) SelectDifficulty(j, toggle); });
                    i++;
                }
            }
            _topic = 1;
            _difficulty = 0;
            SetDifficulty(0);
        }

        void SetDifficulty(int i)
        {
            _wordMin = difficulties[i].words.x;
            _wordMax = difficulties[i].words.y;
            _hide = difficulties[i].hidePercentage;
            _manual = difficulties[i].manualSelection;
            _expMultiplier = difficulties[i].EXPMultiplier;
        }

        public void SelectTopic(int index, Toggle self)
        {
            _topic = index;
            foreach (Transform child in topicButtonsParent)
            {
                Toggle toggle = child.GetComponent<Toggle>();
                if (toggle && toggle != self)
                {
                    toggle.isOn = false;
                }
            }
        }

        public void SelectDifficulty(int index, Toggle self)
        {
            _difficulty = index;
            SetDifficulty(index);
            foreach (Transform child in difficultyButtonsParent)
            {
                Toggle toggle = child.GetComponent<Toggle>();
                if (toggle && toggle != self)
                {
                    toggle.isOn = false;
                }
            }
        }
    }
}
