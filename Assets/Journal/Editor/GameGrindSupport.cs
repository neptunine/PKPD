using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameGrindSupport : EditorWindow {

    [MenuItem("Window/Journal/Support")]
    static void InitializeSupport()
    {
        GameGrindSupport support = (GameGrindSupport)EditorWindow.GetWindow(typeof(GameGrindSupport));
        support.maxSize = new Vector2(700, 273);
        support.minSize = new Vector2(600, 273);
        support.maximized = true;
        support.titleContent = new GUIContent("Journal Help");
        support.Show();

    }
    private void OnGUI()
    {
        GUILayout.Space(5);
        GUILayout.Label("Journal V1.1\nI'm working very hard to make improvements, but silly bugs get through. " +
            "If you're experiencing\na bug, please report it using the email below. I do my best to respond to emails within 24-48 hours.");
        GUILayout.Space(5);
        EditorGUILayout.LabelField("Email Support", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
        EditorGUILayout.LabelField("Sending me a direct email will ensure that I can assist you quickly", EditorStyles.centeredGreyMiniLabel, GUILayout.Height(35));
        GUI.color = new Color32(145, 255, 205, 255);
        if (GUILayout.Button("Send Email\nawfulmedia@gmail.com", GUILayout.Height(35), GUILayout.Width(150)))
        {
            Application.OpenURL("mailto:awfulmedia@gmail.com?subject=[JournalSupport]");
        }
        GUI.color = Color.white;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("Other", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
        EditorGUILayout.LabelField("Use the instant invite link to join the support channel", EditorStyles.centeredGreyMiniLabel, GUILayout.Height(35));
        GUI.color = new Color32(114, 137, 218, 255);
        if (GUILayout.Button("Discord\njoin now!", GUILayout.Height(35), GUILayout.Width(150)))
        {
            Application.OpenURL("https://discord.gg/UYPdWHF");
        }
        GUI.color = Color.white;
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
        EditorGUILayout.LabelField("Something quick? @ me on Twitter", EditorStyles.centeredGreyMiniLabel, GUILayout.Height(35));
        if (GUILayout.Button("Twitter\n@austintgregory", GUILayout.Height(35), GUILayout.Width(150)))
        {
            Application.OpenURL("https://twitter.com/austintgregory");
        }
        EditorGUILayout.EndHorizontal();
    }
}
