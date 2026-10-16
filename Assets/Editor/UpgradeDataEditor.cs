using System;
using System.Linq;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UpgradeData))]
public class UpgradeDataEditor : Editor
{
    private SerializedProperty upgradeNameProperty;
    private SerializedProperty descriptionProperty;
    private SerializedProperty iconProperty;
    private SerializedProperty levelsProperty;
    private SerializedProperty weaponDataProperty;
    
    private void OnEnable()
    {
        upgradeNameProperty = serializedObject.FindProperty("upgradeName");
        descriptionProperty = serializedObject.FindProperty("description");
        iconProperty = serializedObject.FindProperty("icon");
        levelsProperty = serializedObject.FindProperty("levels");
        weaponDataProperty = serializedObject.FindProperty("weaponData");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        EditorGUILayout.PropertyField(upgradeNameProperty);
        EditorGUILayout.PropertyField(descriptionProperty);
        EditorGUILayout.PropertyField(iconProperty);
        EditorGUILayout.PropertyField(weaponDataProperty);

        EditorGUILayout.Space();

        DrawLevels();
        
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawLevels()
    {
        EditorGUILayout.LabelField("Levels", EditorStyles.boldLabel);

        for (int i = 0; i < levelsProperty.arraySize; i++)
        {
            SerializedProperty level = levelsProperty.GetArrayElementAtIndex(i);
            
            if (DrawLevel(level, i))
            {
                levelsProperty.DeleteArrayElementAtIndex(i);
                break;
            }
        }

        if (GUILayout.Button("Add Level"))
        {
            levelsProperty.arraySize++;
        }
        
    }

    private bool DrawLevel(SerializedProperty level, int index)
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        EditorGUILayout.LabelField($"Level {index + 1}", EditorStyles.boldLabel);

        SerializedProperty description =
            level.FindPropertyRelative("description");

        SerializedProperty effects =
            level.FindPropertyRelative("effects");

        EditorGUILayout.PropertyField(description);

        EditorGUILayout.LabelField("Effects", EditorStyles.boldLabel);

        for (int i = 0; i < effects.arraySize; i++)
        {
            SerializedProperty effect =
                effects.GetArrayElementAtIndex(i);

            string effectName = effect.managedReferenceValue?.GetType().Name ?? "Mising Effect";
            
            effectName = ObjectNames.NicifyVariableName(effectName);
            effectName = effectName.Replace(" Effect", "");

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.PropertyField(effect, new GUIContent(effectName),true);

            if (GUILayout.Button("-", GUILayout.Width(25)))
            {
                effects.DeleteArrayElementAtIndex(i);
                EditorGUILayout.EndHorizontal();
                break;
            }

            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add Effect"))
        {
            ShowEffectMenu(effects);
        }

        bool removeLevel = GUILayout.Button("Remove Level");

        EditorGUILayout.EndVertical();

        return removeLevel;
    }

    private Type[] GetEffectTypes()
    {
        return TypeCache.GetTypesDerivedFrom<UpgradeEffect>().Where(type => !type.IsAbstract).ToArray();
    }

    private void ShowEffectMenu(SerializedProperty effects)
    {
        GenericMenu menu = new GenericMenu();

        foreach (Type effectType in GetEffectTypes())
        {
            Type capturedType = effectType;

            menu.AddItem(
                new GUIContent(effectType.Name),
                false,
                () => AddEffect(effects, capturedType)
            );
        }

        menu.ShowAsContext();
    }

    private void AddEffect(SerializedProperty effects, Type effectType)
    {
        serializedObject.Update();

        int newIndex = effects.arraySize;
        effects.arraySize++;

        SerializedProperty newEffect =
            effects.GetArrayElementAtIndex(newIndex);

        newEffect.managedReferenceValue =
            Activator.CreateInstance(effectType);

        serializedObject.ApplyModifiedProperties();
    }
}
