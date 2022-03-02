using UnityEngine;
using System.Collections;

public class AchievementManager : MonoBehaviour
{
    public GameObject achievementPrefab;
    void Start()
    {
        CreateAchievement("General");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CreateAchievement(string category)
    {
        GameObject achievement = (GameObject)Instantiate(achievementPrefab);

        SetAchievementInfo(category, achievement);
    }

    public void SetAchievementInfo(string category,GameObject achievement)
    {
        achievement.transform.SetParent(GameObject.Find(category).transform);
    }
}
