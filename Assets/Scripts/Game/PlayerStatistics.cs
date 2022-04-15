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
            level.text = string.Format("{0:N0}", playerData.level);
            currentExp.text = string.Format("{0:N0}", playerData.currentExp);
            levelupExp.text = string.Format("{0:N0}", playerData.expForNextLevel);
            totalExp.text = string.Format("{0:N0}", playerData.totalExp);
            expBar.localScale = new Vector3(1f * playerData.currentExp / playerData.expForNextLevel, inputBar.localScale.y, inputBar.localScale.z);
            t = playerData.totalCorrectInput;
            f = playerData.totalIncorrectInput;
            if (t + f != 0) p = 1f * t / (t + f);
            else p = .5f;
            correctInput.text = string.Format("{0:N0}", t);
            incorrectInput.text = string.Format("{0:N0}", f);
            percentInput.text = string.Format("{0:P0}", p);
            inputBar.localScale = new Vector3(p, inputBar.localScale.y, inputBar.localScale.z);
            t = playerData.totalWordCleared;
            f = playerData.totalWordFailed;
            if (t + f != 0) p = 1f * t / (t + f);
            else p = .5f;
            wordCleared.text = string.Format("{0:N0}", t);
            wordFailed.text = string.Format("{0:N0}", f);
            percentWord.text = string.Format("{0:P0}", p);
            wordBar.localScale = new Vector3(p, wordBar.localScale.y, wordBar.localScale.z);
            t = playerData.totalLevelCleared;
            f = playerData.totalLevelFailed;
            if (t + f != 0) p = 1f * t / (t + f);
            else p = .5f;
            levelCleared.text = string.Format("{0:N0}", t);
            levelFailed.text = string.Format("{0:N0}", f);
            percentLevel.text = string.Format("{0:P0}", p);
            levelBar.localScale = new Vector3(p, levelBar.localScale.y, levelBar.localScale.z);
            timePlayed.text = string.Format("{0:F1} Hours", playerData.timePlayed.TotalHours);
        }
    }
}
