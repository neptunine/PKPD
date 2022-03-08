using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class EditorTMProHandler
{
    private string folderPath;

    public string GetTMProPackagePath()
    {
        // Check for potential UPM package
        string packagePath = Path.GetFullPath("Packages/com.unity.textmeshpro");
        if (Directory.Exists(packagePath))
        {
            return packagePath;
        }

        packagePath = Path.GetFullPath("Assets/..");
        if (Directory.Exists(packagePath))
        {
            // Search default location for development package
            if (Directory.Exists(packagePath + "/Assets/Packages/com.unity.TextMeshPro/Editor Resources"))
            {
                return packagePath + "/Assets/Packages/com.unity.TextMeshPro";
            }

            // Search for default location of normal TextMesh Pro AssetStore package
            if (Directory.Exists(packagePath + "/Assets/TextMesh Pro/Editor Resources"))
            {
                return packagePath + "/Assets/TextMesh Pro";
            }

            // Search for potential alternative locations in the user project
            string[] matchingPaths = Directory.GetDirectories(packagePath, "TextMesh Pro", SearchOption.AllDirectories);
            string path = ValidateLocation(matchingPaths, packagePath);
            if (path != null) return packagePath + path;
        }
        return null;
    }

    public string ValidateLocation(string[] paths, string projectPath)
    {
        for (int i = 0; i < paths.Length; i++)
        {
            // Check if any of the matching directories contain a GUISkins directory.
            if (Directory.Exists(paths[i] + "/Editor Resources"))
            {
                folderPath = paths[i].Replace(projectPath, "");
                folderPath = folderPath.TrimStart('\\', '/');
                return folderPath;
            }
        }
        return null;
    }

    public void OnPackageImportComplete(string packageName)
    {
        if (packageName == "TMP Essential Resources")
        {
            Debug.Log(packageName + " imported successfully.");
            AssetDatabase.importPackageCompleted -= OnPackageImportComplete;
        }
    }

    public void OnPackageImportFail(string packageName, string errorMessage)
    {
        if (packageName == "TMP Essential Resources")
        {
            Debug.Log(packageName + " failed to import. Reason: " + errorMessage);
            AssetDatabase.importPackageCompleted -= OnPackageImportComplete;
        }
    }
}
