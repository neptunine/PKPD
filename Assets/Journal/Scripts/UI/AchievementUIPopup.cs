using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Unassigned warnings due to [SerializeField] private fields being assigned in inspector
#pragma warning disable 649

namespace Journal
{
    [DisallowMultipleComponent]
    public class AchievementUIPopup : MonoBehaviour
    {
        [SerializeField]
        private JournalSettings settings;
        [SerializeField]
        private TextMeshProUGUI popupTitleText, popupDescriptionText;
        [SerializeField]
        private Image achievementIconPopup;
        [HideInInspector]
        private Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();
            GetComponent<AudioSource>().clip = settings.grantAudioClip;
            popupTitleText.color = settings.accentColor;
            popupDescriptionText.color = Color.white;
            GetComponent<Image>().color = settings.baseColor;
        }

        public void SetAchievementValues(Achievement achievement)
        {
            animator.Play("Achievement_Popup_Base_Animation", 0, 0f);
            string path = achievement.trimmedIconPath;
            achievementIconPopup.sprite = Resources.Load<Sprite>(path);
            popupTitleText.text = string.Format("{0} completed!", achievement.title);
            popupDescriptionText.text = achievement.description;
        }
    }
}
