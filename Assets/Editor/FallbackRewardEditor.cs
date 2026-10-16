using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FallbackReward))]
public class FallbackRewardEditor : Editor
{
    private SerializedProperty rewardNameProperty;
    private SerializedProperty effectProperty;

    private void OnEnable()
    {
        rewardNameProperty = serializedObject.FindProperty("rewardName");
        effectProperty = serializedObject.FindProperty("effect");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(rewardNameProperty);

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Reward Effect", EditorStyles.boldLabel);

        if (effectProperty.managedReferenceValue == null)
        {
            EditorGUILayout.HelpBox(
                "No effect assigned.",
                MessageType.Info
            );
        }
        else
        {
            Type effectType = effectProperty.managedReferenceValue.GetType();

            EditorGUILayout.LabelField(
                ObjectNames.NicifyVariableName(effectType.Name),
                EditorStyles.boldLabel
            );

            EditorGUILayout.PropertyField(
                effectProperty,
                GUIContent.none,
                true
            );
        }

        if (GUILayout.Button("Select Effect"))
        {
            ShowEffectMenu();
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void ShowEffectMenu()
    {
        GenericMenu menu = new GenericMenu();

        Type[] effectTypes = TypeCache
            .GetTypesDerivedFrom<UpgradeEffect>()
            .Where(type => !type.IsAbstract)
            .ToArray();

        foreach (Type effectType in effectTypes)
        {
            Type capturedType = effectType;

            menu.AddItem(
                new GUIContent(
                    ObjectNames.NicifyVariableName(effectType.Name)
                ),
                false,
                () => SetEffect(capturedType)
            );
        }

        menu.AddSeparator("");

        menu.AddItem(
            new GUIContent("Remove Effect"),
            false,
            RemoveEffect
        );

        menu.ShowAsContext();
    }

    private void SetEffect(Type effectType)
    {
        serializedObject.Update();

        effectProperty.managedReferenceValue =
            Activator.CreateInstance(effectType);

        serializedObject.ApplyModifiedProperties();
    }

    private void RemoveEffect()
    {
        serializedObject.Update();

        effectProperty.managedReferenceValue = null;

        serializedObject.ApplyModifiedProperties();
    }
}