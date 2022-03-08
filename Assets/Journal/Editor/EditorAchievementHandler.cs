using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace GameGrind
{
    [System.Serializable]
    public class EditorAchievementHandler
    {

        public List<Achievement> LoadAchievementFile(bool demo = false)
        {
            string loadedJSON = "";
            // If no Achievement.json file, load the demo achievement data.
            if (File.Exists(Application.dataPath + "/Journal/Resources/JSON/Achievements.json"))
            {
                loadedJSON = File.ReadAllText(Application.dataPath + "/Journal/Resources/JSON/Achievements.json");
            }
            else
            {
                loadedJSON = File.ReadAllText(Application.dataPath + "/Journal/Resources/JSON/Demo-Achievements.json");
                
            }
            EditorAchievementList achievements = JsonUtility.FromJson<EditorAchievementList>(loadedJSON);
            // After loading the data, save it to make sure icon instanceIDs are fine
            // If no Achievements.json exists, this will create one using the demo data
            SaveFromEditor(achievements.AchievementList);
            return new List<Achievement>(achievements.AchievementList);
        }

        /// <summary>
        /// Save JSON modifications to the Resources folder. This should only be done in the editor.
        /// </summary>
        public void SaveFromEditor(List<Achievement> achievementList)
        {
            // Grab the absolute path to the JSON data in Resources
            string path = Application.dataPath + "/Journal/Resources/JSON/";

            // Write out the serialized data to the Achievements.json file, overwriting it if it exists (it's okay, we have backup!)
            //File.WriteAllText(path + "Achievements.json", Newtonsoft.Json.JsonConvert.SerializeObject(achievementList, Newtonsoft.Json.Formatting.Indented));

            // Take the data in our achievement list and convert it into
            // Human readable JSON. Remove the formatting to optimize the file.
            EditorAchievementList achievementCollection = new EditorAchievementList();
            achievementCollection.AchievementList = achievementList;
            string jsonString = JsonUtility.ToJson(achievementCollection, true);
            File.WriteAllText(path + "/Achievements.json", jsonString);

            // Make a back-up of the existing JSON data (you know, just in case...)
            if (File.Exists(path + "/Achievements.json"))
            {
                File.Copy(path + "/Achievements.json", path + "Achievements-Backup.json", true);
            }
        }
    }

    [System.Serializable]
    public class EditorAchievementList
    {
        public List<Achievement> AchievementList;
    }
}
