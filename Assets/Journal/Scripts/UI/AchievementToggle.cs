using UnityEngine;

namespace GameGrind
{
    public class AchievementToggle : MonoBehaviour
    {
        private JournalCanvas journalCanvas;
        private AchievementUIList achievementPanel;
        void Start()
        {
            journalCanvas = FindObjectOfType<JournalCanvas>();
            this.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => journalCanvas.ToggleAchievementPanel());
        }
    }
}
