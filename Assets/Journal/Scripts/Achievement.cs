namespace Journal
{
    [System.Serializable]
    public class Achievement
    {
        public int id;
        public string title;
        public UnityEngine.Sprite icon;
        public string iconPath;
        public string trimmedIconPath;
        public string description;
        public int value;
        public int neededValue;
        public bool displayAsPercentage;
        public int points;
        public bool completed;
        public bool secret;

        // Construct that the JSON serializer uses to build achievement database
        public Achievement(int id, string title, UnityEngine.Sprite icon, int value, string description, int neededValue, bool displayAsPercentage, int points, bool secret, string iconPath = "")
        {
            // unique identifier for all achievements
            this.id = id;
            // name for achievement, also unique
            this.title = title;
            // relative icon path "Assets/Journal/Resources/Sprites/SpriteName.png" used for editor icon loading
            this.iconPath = iconPath;
            if (iconPath != "")
                this.icon = UnityEngine.Resources.Load<UnityEngine.Sprite>(iconPath);
            else
                this.icon = icon;
            UnityEngine.Debug.Log(iconPath);
            this.value = value;
            this.description = description;
            this.neededValue = neededValue;
            this.displayAsPercentage = displayAsPercentage;
            this.points = points;
            this.completed = value >= neededValue;
            this.secret = secret;
        }

        // Constructor for creating new achievements, avoiding a blank canvas
        public Achievement()
        {
            this.id = Journal.achievementMaster.Count + 1;
            this.title = "New achievement";
            this.icon = null;
            this.iconPath = "";
            this.value = 0;
            this.description = "This is the description for your new achievement.";
            this.neededValue = 25;
            this.points = 10;
            this.completed = value >= neededValue;
            this.secret = false;
        }
    }

    // Wrap class used to serialize progress data to save files
    [System.Serializable]
    public class AchievementProgressData
    {
        public int id;
        public int value;
        public bool secret;
        public AchievementProgressData(int id, int value, bool secret)
        {
            this.id = id;
            this.value = value;
            this.secret = secret;
        }
    }
}