using UnityEngine;
using UnityEditor;


public class AchievementInstaller : EditorWindow
{
    static void Install()
    {
        if (GameObject.Find("JournalCanvas") == null)
        {
            PrefabUtility.InstantiatePrefab(Resources.Load<GameObject>("Prefabs/JournalCanvas"));
            Debug.Log("Journal was installed. Journal should only be installed in the first scene you want achievement tracking in as it will carry over to loaded scenes.");
        }
        else
        {
            Debug.LogWarning("An instance of Journal is already installed. To remove it, simply delete the JournalCanvas object.");
        }
        if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            Debug.Log("An EventSystem was created as one was not detected in your scene.");
            PrefabUtility.InstantiatePrefab(Resources.Load<UnityEngine.EventSystems.EventSystem>("Prefabs/EventSystem"));
        }
        /*
        if (GameObject.FindObjectOfType<Canvas>())
        {
            if (!GameObject.Find("Achievement_Popup"))
            {
                GameObject achievementPopup = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load<GameObject>("Prefabs/Achievement_Popup"));
                achievementPopup.transform.SetParent(GameObject.FindObjectOfType<Canvas>().transform);
                achievementPopup.GetComponent<RectTransform>().position = 
                    new Vector2(GameObject.FindObjectOfType<Canvas>().GetComponent<RectTransform>().anchoredPosition.x,
                    GameObject.FindObjectOfType<Canvas>().GetComponent<RectTransform>().anchoredPosition.y 
                    + (GameObject.FindObjectOfType<Canvas>().GetComponent<RectTransform>().rect.height / 2)
                    + (achievementPopup.GetComponent<RectTransform>().rect.height/2));
            }
            if (!GameObject.Find("Achievement_UI_List"))
            {
                GameObject achievementList = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load<GameObject>("Prefabs/Achievement_UI_List"));
                achievementList.transform.SetParent(GameObject.FindObjectOfType<Canvas>().transform);
                achievementList.transform.position = new Vector2(GameObject.FindObjectOfType<Canvas>().GetComponent<RectTransform>().anchoredPosition.x,
                    GameObject.FindObjectOfType<Canvas>().GetComponent<RectTransform>().anchoredPosition.y);
            }
        }
        else
        {
            Debug.LogWarning("Couldn't install Journal. Installer requires a UI Canvas object to work. Refer to the included README file for more information.");
        }
    */
    }
}