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
            totalexp,
            totalLivesTaken,
            timePlayed,
            totalWordCleared,
            totalWordFailed,
            totalLevelCleared;


        private void OnEnable()
        {
            level.text = playerData.level.ToString();
            totalexp.text = playerData.totalExp.ToString();
            totalLivesTaken.text = playerData.totalLivesTaken.ToString();
            totalWordCleared.text = playerData.totalWordCleared.ToString();
            totalWordFailed.text = playerData.totalWordFailed.ToString();
            totalLevelCleared.text = playerData.totalWordFailed.ToString();
            timePlayed.text = $"{playerData.timePlayed.Days}d {playerData.timePlayed.Hours}h {playerData.timePlayed.Minutes}m";
        }
    }
}
