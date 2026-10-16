using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(UpgradeSelector))]
public class UpgradeSelectorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Refresh Available Upgrades"))
        {
            string[] guids = AssetDatabase.FindAssets("t:UpgradeData");

            List<UpgradeData> upgrades = new();

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);

                UpgradeData upgrade = AssetDatabase.LoadAssetAtPath<UpgradeData>(path);

                if (upgrade != null)
                {
                    upgrades.Add(upgrade);
                }
            }
            
            SerializedProperty availableUpgrades = serializedObject.FindProperty("availableUpgrades");
            
            availableUpgrades.ClearArray();

            foreach (UpgradeData upgrade in upgrades)
            {
                int index = availableUpgrades.arraySize;
                
                availableUpgrades.InsertArrayElementAtIndex(index);
                
                availableUpgrades.GetArrayElementAtIndex(index).objectReferenceValue = upgrade;
                
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
