using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using TMPro;
#pragma warning disable 649
namespace GameGrind
{
    public class AchievementUIElement : MonoBehaviour
    {
        [SerializeField]
        private JournalSettings settings;

        [SerializeField]
        private TextMeshProUGUI titleText, descriptionText, valueText, rewardText;
        [SerializeField]
        private Slider valueSlider;
        [SerializeField]
        private Image sliderFill;
        [SerializeField]
        private Image valueBackground;
        [SerializeField]
        private Image sliderBackground;
        [SerializeField]
        private Image iconImage;


        private void Awake()
        {
            titleText.color = settings.accentColor;
            rewardText.color = settings.accentColor;
            GetComponent<Image>().color = settings.baseColor;
        }

        /// <summary>
        /// Assign the achievement UI element the appropriate achievement details
        /// </summary>
        /// <param name="achievement">The achievement assigned to this element</param>
        public void SetAchievementValues(Achievement achievement)
        {
            if (iconImage != null)
            {
                iconImage.sprite = Resources.Load<Sprite>(achievement.trimmedIconPath);
            }
            if (achievement.secret && !achievement.completed)
            {
                titleText.text = settings.hiddenAchievementTitle;
                descriptionText.text = settings.hiddenAchievementDescription;
                rewardText.text = settings.secretShowReward ? achievement.points.ToString() : "";
                valueText.text = "";
                valueBackground.color = settings.secretValueBackgroundColor;
                descriptionText.alignment = titleText.alignment = settings.secretTextAlignment;
                sliderFill.color = sliderBackground.color = settings.progressBarSecretiveColor;
            }
            else if (achievement.secret && achievement.completed || !achievement.secret)
            {
                titleText.text = achievement.title;
                descriptionText.text = achievement.description;
                rewardText.text = achievement.points.ToString();
                // If the achievement is a Percentage achievement, show a percentage value in the UI
                if (achievement.displayAsPercentage)
                {
                    valueText.text = string.Format("{0}%", ((float)achievement.value / (float)achievement.neededValue) * 100);
                }
                // If it's not, show the standard display values "This out of that, e.g. 10/15"
                else
                {
                    valueText.text = string.Format("{0}/{1}", achievement.value, achievement.neededValue);
                }

                /*
                    Our progress bar is based 0 - 100. We calculate a percentage
                    And assign the result to the progress bar
                */
                valueSlider.value = ((float)achievement.value / (float)achievement.neededValue) * 100;
                descriptionText.alignment = titleText.alignment = TextAlignmentOptions.MidlineLeft;
                valueBackground.color = settings.ValueBackgroundColor;
                sliderFill.color = settings.progressBarColor;
                sliderBackground.color = settings.progressBarBackgroundColor;
                // If the updated achievement is completed, color the progress bar color cause yay!
                if (achievement.completed)
                    sliderFill.color = settings.progressBarCompleteColor;
            }
        }
    }
}