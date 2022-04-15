using UnityEngine;

// Unassigned warnings due to [SerializeField] private fields being assigned in inspector
#pragma warning disable 649

namespace Journal
{
    public class JournalCanvas : MonoBehaviour
    {
        [SerializeField]
        private KeyCode togglePanel;
        private AchievementUIList panel;
        void Awake()
        {
            if (FindObjectsOfType<JournalCanvas>().Length > 1)
            {
                Destroy(this.gameObject);
                Debug.LogWarning("Deleted duplicate instance of Journal. Journal should only be installed in the first scene you load achievements in and not in a scene that's revisted often.");
            }
            // We're finding the list by name as a child to make sure we find it even if it isn't disabled
            panel = transform.Find("Achievement_UI_List").GetComponent<AchievementUIList>();
            DontDestroyOnLoad(this);
        }

        private void Update()
        {
            if (Input.GetKeyDown(togglePanel))
            {
                //if (panel != null)
                    ToggleAchievementPanel();
            }
        }

        public void ToggleAchievementPanel()
        {
            panel.TogglePanel();
        }
    }
}
