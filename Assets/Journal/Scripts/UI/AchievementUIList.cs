using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

// Unassigned warnings due to [SerializeField] private fields being assigned in inspector
#pragma warning disable 649

namespace GameGrind
{
    public class AchievementUIList : MonoBehaviour
    {
        [SerializeField]
        private JournalSettings settings;
        [SerializeField]
        private Image panelHeader, exitButton, scrollBackground, scrollFill;
        [SerializeField]
        private GameObject listElementPrefab;
        [SerializeField]
        private Transform listUIPanel;
        [SerializeField]
        private TextMeshProUGUI currentAchievementScore;

        private bool isPanelActive = false;
        private List<AchievementUIElement> achievementUIObject = new List<AchievementUIElement>();

        private void Awake()
        {
            // Assign all base colored objects the baseColor
            GetComponent<Image>().color = panelHeader.color = scrollBackground.color = settings.baseColor;
            // Make the scrollbar fill a bit lighter than the base color
            scrollFill.color = settings.baseColor * 1.25f;
            // Assign the exit button the defined color
            exitButton.color = settings.exitButtonColor;
        }

        // Use this for initialization
        void Start()
        {
            // Make sure we start at 0 score when the system is loaded
            AchievementController.CurrentAchievementScore = 0;
            gameObject.SetActive(true);
            BuildStatListUI();
            AchievementEvents.AchievementValueChanged += UpdateAchievementUIData;
            AchievementEvents.AchievementGranted += UpdateScore;
            currentAchievementScore.text = AchievementController.CurrentAchievementScore.ToString() + " " + settings.achievementPointValueName;

            // Reloading the canvas since it is a persistent object can break the prefab references so we enforce the event listener directly
            transform.Find("Close_Button").GetComponent<Button>().onClick.AddListener(() => gameObject.SetActive(false));
        }

        /// <summary>
        /// Build the List UI panel, populating it with Stat Data
        /// </summary>
        public void BuildStatListUI()
        {
            for (int i = 0; i < Journal.achievementMaster.Count; i++)
            {
                // Instantiate and set the UI element's parent to the scroll rect of our list panel
                achievementUIObject.Add(Instantiate(settings.achievementUIElement));
                achievementUIObject[i].transform.SetParent(listUIPanel, false);
                if (Journal.achievementMaster[i].completed)
                    AchievementController.CurrentAchievementScore += Journal.achievementMaster[i].points;
            }
            UpdateStatListData();

            // Journal is setup and ready to go
            AchievementEvents.OnJournalReady();
        }

        /// <summary>
        /// Refresh displayed stat values.
        /// </summary>
        public void UpdateStatListData()
        {
            for (int i = 0; i < achievementUIObject.Count; i++)
            {
                achievementUIObject[i].SetAchievementValues(Journal.achievementMaster[i]);
                // Shade every other row for readability
                if (i % 2 == 0)
                    achievementUIObject[i].GetComponent<Image>().color = settings.AltRowShading;
            }
        }

        /// <summary>
        /// Refresh displayed stat values.
        /// </summary>
        public void UpdateAchievementUIData(Achievement achievement)
        {
            for (int i = 0; i < Journal.achievementMaster.Count; i++)
            {
                if (Journal.achievementMaster[i].id == achievement.id)
                    achievementUIObject[i].SetAchievementValues(achievement);
            }
        }

        /// <summary>
        /// Update the achievement list's point display
        /// </summary>
        public void UpdateScore(Achievement achievement)
        {
            currentAchievementScore.text = string.Format("{0} {1}", AchievementController.CurrentAchievementScore, settings.achievementPointValueName);
        }

        /// <summary>
        /// Toggle the panel's active state
        /// </summary>
        public void TogglePanel()
        {
            isPanelActive = !isPanelActive;
            this.gameObject.SetActive(isPanelActive);
        }

        private void ClearAchievementList()
        {
            for (int i = 0; i < achievementUIObject.Count; i++)
            {
                Destroy(achievementUIObject[i].gameObject);
            }
        }
    }
}
