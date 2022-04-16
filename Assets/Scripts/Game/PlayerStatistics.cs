using UnityEngine;
using UnityEngine.UI;
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
        public Slider expBar;

        [Header("time")]
        public TMP_Text timePlayed;

        [Header("input")]
        public TMP_Text correctInput;
        public TMP_Text incorrectInput;
        public TMP_Text percentInput;
        public Slider inputBar;

        [Header("word")]
        public TMP_Text wordCleared;
        public TMP_Text wordFailed;
        public TMP_Text percentWord;
        public Slider wordBar;

        [Header("level")]
        public TMP_Text levelCleared;
        public TMP_Text levelFailed;
        public TMP_Text percentLevel;
        public Slider levelBar;


        private void OnEnable()
        {
            int t, f;
            float p;
            level.text = string.Format("{0:N0}", playerData.level);
            currentExp.text = string.Format("{0:N0}", playerData.currentExp);
            levelupExp.text = string.Format("{0:N0}", playerData.expForNextLevel);
            totalExp.text = string.Format("{0:N0}", playerData.totalExp);
            expBar.value = 1f * playerData.currentExp / playerData.expForNextLevel;
            t = playerData.totalCorrectInput;
            f = playerData.totalIncorrectInput;
            if (t + f != 0) p = 1f * t / (t + f);
            else p = .5f;
            correctInput.text = string.Format("{0:N0}", t);
            incorrectInput.text = string.Format("{0:N0}", f);
            percentInput.text = string.Format("{0:P0}", p);
            inputBar.value = p;
            t = playerData.totalWordCleared;
            f = playerData.totalWordFailed;
            if (t + f != 0) p = 1f * t / (t + f);
            else p = .5f;
            wordCleared.text = string.Format("{0:N0}", t);
            wordFailed.text = string.Format("{0:N0}", f);
            percentWord.text = string.Format("{0:P0}", p);
            wordBar.value = p;
            t = playerData.totalLevelCleared;
            f = playerData.totalLevelFailed;
            if (t + f != 0) p = 1f * t / (t + f);
            else p = .5f;
            levelCleared.text = string.Format("{0:N0}", t);
            levelFailed.text = string.Format("{0:N0}", f);
            percentLevel.text = string.Format("{0:P0}", p);
            levelBar.value = p;
            timePlayed.text = string.Format("{0:F1} Hours", playerData.timePlayed.TotalHours);
        }
    }
}
