using UnityEngine;
using TMPro;

namespace Game {
    public class PlayerStatistics : MonoBehaviour
    {
        [SerializeField]
        private PlayerData
            playerData;

        public TMP_Text
            level,
            currentExp,
            totalExp,
            totalLivesTaken,
            timePlayed,
            totalWordCleared,
            totalWordFailed,
            totalLevelCleared,
            totalLevelFailed;


        private void OnEnable()
        {
            level.text = playerData.level.ToString();
            currentExp.text = $"{playerData.currentExp}/{playerData.expForNextLevel}";
            totalExp.text = playerData.totalExp.ToString();
            totalLivesTaken.text = playerData.totalLivesTaken.ToString();
            totalWordCleared.text = playerData.totalWordCleared.ToString();
            totalWordFailed.text = playerData.totalWordFailed.ToString();
            totalLevelCleared.text = playerData.totalLevelCleared.ToString();
            totalLevelFailed.text = playerData.totalLevelFailed.ToString();
            timePlayed.text = $"{playerData.timePlayed.Days}d {playerData.timePlayed.Hours}h {playerData.timePlayed.Minutes}m";
        }
    }
}
