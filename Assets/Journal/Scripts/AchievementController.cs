using UnityEngine;

namespace Journal
{
    public class AchievementController
    {
        public static int CurrentAchievementScore { get; set; }
        /// <summary>
        /// Check a achievement to see if the required value was met.
        /// </summary>
        /// <param name="achievement">The achievement to check.</param>
        /// <returns></returns>
        public static void CheckForCompletion(Achievement achievement)
        {
            // Set the completed value to true if so, and also return true
            // Set to false and return false if it wasn't met
            if (!achievement.completed && achievement.value >= achievement.neededValue)
            {
                achievement.completed = true;
                CurrentAchievementScore += achievement.points;
                Grant(achievement);
            }
            else
            {
                achievement.completed = false;
            }
        }

        /// <summary>
        /// Grant an achievement.
        /// This also completes the achievement regardless of progress.
        /// </summary>
        public static void Grant(Achievement achievement)
        {
            // Set the value to the required value
            // Set the completed value to true, as completed
            achievement.value = achievement.neededValue;

            achievement.completed = true;
            AchievementEvents.OnAchievementValueChanged(achievement);
            AchievementEvents.OnAchievementGranted(achievement);
        }

        /// <summary>
        /// Revoke an achievement.
        /// </summary>
        /// <param name="stat">Stat to reset completion.</param>
        /// <param name="resetValue">Reset stat value?</param>
        public static void Revoke(Achievement achievement, bool resetValue)
        {
            achievement.completed = false;
            if (resetValue)
                achievement.value = 0;
            CurrentAchievementScore -= achievement.points;
        }

    }
}
