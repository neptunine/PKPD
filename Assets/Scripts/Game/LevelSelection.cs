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

        public int
            topic,
            difficulty;

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
            topic = 1;
            difficulty = 0;
        }

        void Update()
        {

        }

        public void SelectTopic(int index, Toggle self)
        {
            topic = index;
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
            difficulty = index;
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
