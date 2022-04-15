using UnityEngine;
using System.Collections;

namespace Journal
{
    [DisallowMultipleComponent]
    public class AchievementGranter : MonoBehaviour
    {
        AchievementUIPopup achievementPopup;
        // Audio source for handling sound played on Achievement Grant
        AudioSource audioSource;
        [SerializeField] Game.AudioController audioController;

        void Start()
        {
            achievementPopup = GetComponent<AchievementUIPopup>();
            //audioSource = GetComponent<AudioSource>();

            // Subscribe to the OnAchievementGrant event so we know when an achievement was completed
            AchievementEvents.AchievementGranted += ShowAchievementGrant;
        }

        /// <summary>
        /// Display the achievement Popup and play a boop
        /// </summary>
        /// <param name="achievement"></param>
        public void ShowAchievementGrant(Achievement achievement)
        {
            //audioSource.Play();
            audioController.SetPlayAchievementGrant();
            achievementPopup.SetAchievementValues(achievement);
        }
    }
}
