using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Journal
{
    //[CreateAssetMenu(fileName = "Settings", menuName = "Journal/Settings File")]
    public class JournalSettings : ScriptableObject
    {
        // Properties are used to get a calculated value here
        public Color AltRowShading
        {
            get
            {
                return new Color(baseColor.r * .85f, baseColor.g * .85f, baseColor.b * .85f, baseColor.a);
            }
        }
        public Color ValueBackgroundColor
        {
            get
            {
                return new Color(baseColor.r, baseColor.g, baseColor.b, .55f);
            }
        }

        [Header("System")]
        [Tooltip("The achievement UI element to use in the achievement list")]
        public AchievementUIElement achievementUIElement;
        [Tooltip("Sound effect to play when an achievement is granted")]
        public AudioClip grantAudioClip;

        [Tooltip("Name of the points awarded for achievements")]
        public string achievementPointValueName;

        [Tooltip("Relative save path: persistentDataPath[/saves/]")]
        public string saveDataPath;

        [Tooltip("Name of the achievement save file, e.g. achievements-save")]
        public string saveFileName;

        [Tooltip("Auto Save will save progress automatically when Journal is destroyed or app is closed")]
        public bool autoSave;

        [Header("Element Colors")]
        [Tooltip("Base color for UI (background elements)")]
        public Color baseColor;

        [Tooltip("Accent color for headings and accent elements")]
        public Color accentColor;

        [Tooltip("Color of the progress bar fill")]
        public Color progressBarColor;

        [Tooltip("Color of progress bar fill once achievement is granted")]
        public Color progressBarCompleteColor;

        [Tooltip("Color of the progress bar background")]
        public Color progressBarBackgroundColor;

        [Tooltip("Color of the exit button's image")]
        public Color exitButtonColor;

        [HideInInspector]
        public readonly Color valueBackgroundColor;

        [HideInInspector]
        public readonly Color altRowShading;


        [Header("Secret Achievements Options")]
        [Tooltip("Adjust text alignment for 'This achievement is hidden'")]
        public TextAlignmentOptions secretTextAlignment;

        [Tooltip("Achievement title to display for hidden achievements")]
        public string hiddenAchievementTitle;

        [Tooltip("Achievement description to display for hidden achievements")]
        public string hiddenAchievementDescription;

        [Tooltip("Color of the progress bar if achievement is marked 'hidden'")]
        public Color progressBarSecretiveColor;

        [Tooltip("Color of the image background for the progress label if achievement is marked 'hidden'")]
        public Color secretValueBackgroundColor;

        [Tooltip("If true, achievement reward values are displayed for secret achievements")]
        public bool secretShowReward;
    }
}
