using UnityEngine;
using TMPro;

namespace Game {
    public class PlayerStatistics : MonoBehaviour
    {
        [SerializeField] private PlayerData playerData;

        [Header("level")]
        public TMP_Text level;
        public TMP_Text currentExp;
        public TMP_Text levelupExp;
        public TMP_Text totalExp;
        public RectTransform expBar;

        [Header("time")]
        public TMP_Text timePlayed;

        [Header("input")]
        public TMP_Text correctInput;
        public TMP_Text incorrectInput;
        public TMP_Text percentInput;
        public RectTransform inputBar;

        [Header("word")]
        public TMP_Text wordCleared;
        public TMP_Text wordFailed;
        public TMP_Text percentWord;
        public RectTransform wordBar;

        [Header("level")]
        public TMP_Text levelCleared;
        public TMP_Text levelFailed;
        public TMP_Text percentLevel;
        public RectTransform levelBar;


        private void OnEnable()
        {
            int t, f;
            float p;
            level.text = playerData.level.ToString();
            currentExp.text = playerData.currentExp.ToString();
            levelupExp.text = playerData.expForNextLevel.ToString();
            totalExp.text = playerData.totalExp.ToString();
            t = playerData.totalCorrectInput;
            f = playerData.totalIncorrectInput;
            if (t + f != 0) p = 1f * t / (t + f);
            else p = .5f;
            correctInput.text = t.ToString();
            incorrectInput.text = f.ToString();
            percentInput.text = string.Format("{0:P2}", p);
            inputBar.localScale = new Vector3(p, inputBar.localScale.y, inputBar.localScale.z);
            t = playerData.totalWordCleared;
            f = playerData.totalWordFailed;
            if (t + f != 0) p = 1f * t / (t + f);
            else p = .5f;
            wordCleared.text = t.ToString();
            wordFailed.text = f.ToString();
            percentWord.text = string.Format("{0:P2}", p);
            wordBar.localScale = new Vector3(p, wordBar.localScale.y, wordBar.localScale.z);
            t = playerData.totalLevelCleared;
            f = playerData.totalLevelFailed;
            if (t + f != 0) p = 1f * t / (t + f);
            else p = .5f;
            levelCleared.text = t.ToString();
            levelFailed.text = f.ToString();
            percentLevel.text = string.Format("{0:P2}", p);
            levelBar.localScale = new Vector3(p, levelBar.localScale.y, levelBar.localScale.z);
            timePlayed.text = string.Format("{0:F2} Hours", playerData.timePlayed.TotalHours);
        }
    }
}
