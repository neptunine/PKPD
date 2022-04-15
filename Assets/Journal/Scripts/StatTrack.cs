using UnityEngine;
using System.Collections.Generic;

// Unassigned warnings due to [SerializeField] private fields being assigned in inspector
#pragma warning disable 649

namespace Journal
{
    public class StatTrack : MonoBehaviour
    {
        [SerializeField]
        private JournalSettings settings;
        private AchievementCollection achievements;
        void Awake()
        {
            //Journal.SaveName = string.Format("{0}{1}", settings.saveFileName, ".json");
            Journal.SaveName = string.Format("{0}", settings.saveFileName);
            Journal.SavePath = string.Format("{0}{1}", Application.persistentDataPath, settings.saveDataPath);

            achievements = new AchievementCollection();
            /* 
               Since the save file is designed to write over the default achievement list
               We want to see if a save exists to decide if we should use the save file
               or load in the default achievement values.
            */
            // Pass our builder method the raw JSON file, loaded through Resources
            // If no Achievement.json file, load the demo achievement data.
            string loadedJSON = "";
            if (Resources.Load<TextAsset>("JSON/Achievements") != null)
            {
                loadedJSON = Resources.Load<TextAsset>("JSON/Achievements").text;
            }
            else
            {
                loadedJSON = Resources.Load<TextAsset>("JSON/Demo-Achievements").text;
            }
            BuildDatabaseFromJSON(loadedJSON);
            if (Journal.SaveExists())
            {
                Journal.Load();
            }
        }

        /// <summary>
        /// Construct the achievement database from the supplied JSON data
        /// </summary>
        /// <param name="json">JSON data</param>
        private void BuildDatabaseFromJSON(string json)
        {
            /* 
                Take the string of JSON and convert it into a list of achievements
                achievementList is a list of Achievement, and our JSON file's
                top-level array contains an achievement in each element
            */
            achievements = JsonUtility.FromJson<AchievementCollection>(json);
            // Add each decoded achievement to the achievement database
            foreach (Achievement achievement in achievements.AchievementList)
            {
                Journal.Create(achievement);
            }
        }

        private void OnDestroy()
        {
            if (settings.autoSave)
            {
                Journal.Save();
            }
        }
    }

    public class AchievementCollection
    {
        public List<Achievement> AchievementList;
    }

    public class AchievementProgressDataCollection
    {
        public List<AchievementProgressData> achievementProgressList;
        public AchievementProgressDataCollection()
        {
            achievementProgressList = new List<AchievementProgressData>();
        }
    }
}