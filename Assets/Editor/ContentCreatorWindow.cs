using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class ContentCreatorWindow : EditorWindow
{
    private int selectedTab;

    private int selectedBehavior;

    private BaseEnemy chaserTemplate;
    private BaseEnemy berserkerTemplate;
    private BaseEnemy chargerTemplate;
    
    private string enemyName = "New Enemy";
    private BaseEnemy enemyTemplate;
    private int enemyHealth = 100;
    private float enemySpeed = 3f;
    private int experienceReward = 1;
    private Color enemyColor = Color.white;
    private Mesh enemyMesh;
    
    // Berserker
    private float enrageThreshold = 0.4f;
    private float enragedSpeed = 5f;

    // Charger
    private float chargeRange = 6f;
    private float windupDuration = 0.75f;
    private float chargeSpeed = 12f;
    private float overshootDistance = 3f;
    private float chargeCooldown = 2f;

    private EnemySpawner targetSpawner;
    private float unlockTime = 0f;
    private int spawnWeight = 1;
    private int finalSpawnWeight = 10;
    private float transitionDuration = 120f;

    private string upgradeAssetName = "New Upgrade";

    [MenuItem("Window/Content Creator")]
    private static void OpenWindow()
    {
        GetWindow<ContentCreatorWindow>("ContentCreatorWindow");
    }

    private void OnGUI()
    {
        selectedTab = GUILayout.Toolbar(selectedTab, new[] { "Enemies", "Upgrades" });
        
        EditorGUILayout.Space(10);

        switch (selectedTab)
        {
            case 0:
                DrawEnemyCreator();
                break;
            case 1:
                DrawUpgradeCreator();
                break;
        }
    }

    private void DrawEnemyCreator()
    {
        EditorGUILayout.LabelField("Enemy Creator", EditorStyles.boldLabel);

        enemyName = EditorGUILayout.TextField("Enemy Name", enemyName);

        EditorGUILayout.LabelField("Behavior", EditorStyles.boldLabel);

        selectedBehavior = GUILayout.Toolbar(selectedBehavior, new[] { "Chaser", "Berserker", "Charger" });

        enemyTemplate = selectedBehavior switch
        {
            0 => FindEnemyTemplate<BaseEnemy>(),
            1 => FindEnemyTemplate<BerserkerEnemy>(),
            2 => FindEnemyTemplate<ChargerEnemy>(),
            _ => null
        };
        
        EditorGUILayout.Space(10);

        enemyHealth = EditorGUILayout.IntField("Max Health", enemyHealth);
        enemySpeed = EditorGUILayout.FloatField("Movement Speed", enemySpeed);
        experienceReward = EditorGUILayout.IntField("XP Reward", experienceReward);
        experienceReward = Mathf.Max(1, experienceReward);
        
        DrawBehaviourSettings();
        
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Appearance", EditorStyles.boldLabel);

        enemyColor = EditorGUILayout.ColorField("Enemy Color", enemyColor);

        enemyMesh = (Mesh)EditorGUILayout.ObjectField("Mesh Override", enemyMesh, typeof(Mesh),
            false);
        
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Spawning", EditorStyles.boldLabel);

        targetSpawner =
            (EnemySpawner)EditorGUILayout.ObjectField("Target Spawner", targetSpawner,
                typeof(EnemySpawner), true);
        
        unlockTime = EditorGUILayout.FloatField("Unlock Time", unlockTime);
        spawnWeight = EditorGUILayout.IntField("Spawn Weight", spawnWeight);
        finalSpawnWeight = EditorGUILayout.IntField("Final Weight", finalSpawnWeight);
        transitionDuration = EditorGUILayout.FloatField("Transition Duration", transitionDuration);
        
        EditorGUILayout.Space(10);

        using (new EditorGUI.DisabledScope(enemyTemplate == null))
        {
            if (GUILayout.Button("Create Enemy"))
            {
                CreateEnemy();
            }
        }
    }
    
    private void DrawBehaviourSettings()
    {
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Behavior Settings", EditorStyles.boldLabel);

        switch (selectedBehavior)
        {
            case 1: // Berserker
                enrageThreshold = EditorGUILayout.Slider(
                    "Enrage Health Threshold", enrageThreshold, 0f, 1f
                );

                enragedSpeed = EditorGUILayout.FloatField(
                    "Enraged Speed", enragedSpeed
                );
                break;

            case 2: // Charger
                chargeRange = EditorGUILayout.FloatField("Charge Range", chargeRange);
                windupDuration = EditorGUILayout.FloatField("Windup Duration", windupDuration);
                chargeSpeed = EditorGUILayout.FloatField("Charge Speed", chargeSpeed);
                overshootDistance = EditorGUILayout.FloatField("Overshoot Distance", overshootDistance);
                chargeCooldown = EditorGUILayout.FloatField("Charge Cooldown", chargeCooldown);
                break;

            default:
                EditorGUILayout.HelpBox(
                    "This behavior has no additional settings.",
                    MessageType.Info
                );
                break;
        }
    }

    private void DrawUpgradeCreator()
    {
        EditorGUILayout.LabelField("Upgrade Creator", EditorStyles.boldLabel);
        
        upgradeAssetName = EditorGUILayout.TextField("Asset Name",  upgradeAssetName);
        
        EditorGUILayout.Space(10);

        if (GUILayout.Button("Create Upgrade"))
        {
            CreateUpgrade();
        }
    }

    private void CreateEnemy()
    {
        if (enemyTemplate == null || string.IsNullOrWhiteSpace(enemyName))
            return;

        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(enemyTemplate.gameObject);

        try
        {
            instance.name = enemyName;

            Health health = instance.GetComponent<Health>();

            if (health != null)
            {
                SerializedObject healthObject = new SerializedObject(health);
                healthObject.FindProperty("maxHealth").intValue = Mathf.Max(1, enemyHealth);
                healthObject.ApplyModifiedPropertiesWithoutUndo();
            }

            BaseEnemy enemy = instance.GetComponent<BaseEnemy>();

            SerializedObject enemyObject = new SerializedObject(enemy);
            enemyObject.FindProperty("moveSpeed").floatValue = Mathf.Max(0f, enemySpeed);
            enemyObject.FindProperty("experienceReward").intValue = Mathf.Max(1, experienceReward);
            enemyObject.ApplyModifiedPropertiesWithoutUndo();
            
            ApplyEnemyAppearance(instance);
            AlignEnemyVisual(instance);
            ApplyBehaviorSettings(enemy);

            string path = AssetDatabase.GenerateUniqueAssetPath($"Assets/Prefabs/Enemies/{enemyName}.prefab");

            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(instance, path);

            if (prefab != null)
            {
                RegisterEnemyWithSpawner(prefab.GetComponent<BaseEnemy>());
            }

            Selection.activeObject = prefab;
            EditorGUIUtility.PingObject(prefab);
        }
        finally
        {
            DestroyImmediate(instance);
        }
    }

    private void CreateUpgrade()
    {
        if (string.IsNullOrEmpty(upgradeAssetName))
        {
            EditorUtility.DisplayDialog("Invalid Name", "Please enter a name for the upgrade.",
                "OK");
            return;
        }

        UpgradeData upgrade = CreateInstance<UpgradeData>();

        string path = $"Assets/Data/Data_{upgradeAssetName}.asset";
        path = AssetDatabase.GenerateUniqueAssetPath(path);

        AssetDatabase.CreateAsset(upgrade, path);
        AssetDatabase.SaveAssets();
        
        Selection.activeObject = upgrade;
        EditorGUIUtility.PingObject(upgrade);
    }

    private void ApplyEnemyAppearance(GameObject instance)
    {
        MeshFilter meshFilter = instance.GetComponentInChildren<MeshFilter>();

        if (enemyMesh != null || meshFilter != null)
        {
            meshFilter.sharedMesh = enemyMesh;
        }
        
        Renderer renderer = instance.GetComponentInChildren<Renderer>();

        if (renderer != null)
        {
            Material sourceMaterial = renderer.sharedMaterial;

            if (sourceMaterial != null)
            {
                Material newMaterial = new Material(sourceMaterial);

                if (newMaterial.HasProperty("_BaseColor"))
                    newMaterial.SetColor("_BaseColor", enemyColor);
                else if (newMaterial.HasProperty("_Color"))
                    newMaterial.SetColor("_Color", enemyColor);

                string materialPath =
                    AssetDatabase.GenerateUniqueAssetPath($"Assets/Materials/MAT_{instance.name}.mat");
                
                AssetDatabase.CreateAsset(newMaterial, materialPath);
                
                renderer.sharedMaterial = newMaterial;
            }
        }
    }

    private void RegisterEnemyWithSpawner(BaseEnemy newEnemy)
    {
        if (targetSpawner == null)
            return;

        SerializedObject spawnerObject = new SerializedObject(targetSpawner);
        SerializedProperty enemyTypes = spawnerObject.FindProperty("enemyTypes");
        
        int index = enemyTypes.arraySize;
        enemyTypes.InsertArrayElementAtIndex(index);

        SerializedProperty entry = enemyTypes.GetArrayElementAtIndex(index);

        entry.FindPropertyRelative("prefab").objectReferenceValue = newEnemy;
        entry.FindPropertyRelative("unlockTime").floatValue = Mathf.Max(0f, unlockTime);
        entry.FindPropertyRelative("startingWeight").intValue = Mathf.Max(1, spawnWeight);
        entry.FindPropertyRelative("finalWeight").intValue = Mathf.Max(1, spawnWeight);
        entry.FindPropertyRelative("transitionDuration").floatValue = 120f;
        
        spawnerObject.ApplyModifiedProperties();

        EditorUtility.SetDirty(targetSpawner);
    }

    private BaseEnemy FindEnemyTemplate<T>() where T : BaseEnemy
    {
        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/Prefabs/Enemies/Templates" });

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (prefab == null)
                continue;

            BaseEnemy enemy = prefab.GetComponent<BaseEnemy>();

            if (enemy != null && enemy.GetType() == typeof(T))
                return enemy; 
        }

        return null;
    }
    
    private void ApplyBehaviorSettings(BaseEnemy enemy)
    {
        SerializedObject enemyObject = new SerializedObject(enemy);

        if (enemy is BerserkerEnemy)
        {
            enemyObject.FindProperty("enrageThreshold").floatValue =
                Mathf.Clamp01(enrageThreshold);

            enemyObject.FindProperty("enragedSpeed").floatValue =
                Mathf.Max(0f, enragedSpeed);
        }
        else if (enemy is ChargerEnemy)
        {
            enemyObject.FindProperty("chargeRange").floatValue =
                Mathf.Max(0f, chargeRange);

            enemyObject.FindProperty("windupDuration").floatValue =
                Mathf.Max(0f, windupDuration);

            enemyObject.FindProperty("chargeSpeed").floatValue =
                Mathf.Max(0f, chargeSpeed);

            enemyObject.FindProperty("overshootDistance").floatValue =
                Mathf.Max(0f, overshootDistance);

            enemyObject.FindProperty("chargeCooldown").floatValue =
                Mathf.Max(0f, chargeCooldown);
        }

        enemyObject.ApplyModifiedPropertiesWithoutUndo();
    }
    
    private void AlignEnemyVisual(GameObject instance)
    {
        MeshFilter meshFilter = instance.GetComponentInChildren<MeshFilter>();
        Collider collider = instance.GetComponent<Collider>();

        if (meshFilter == null || collider == null || meshFilter.sharedMesh == null)
            return;

        Transform visual = meshFilter.transform;

        // Calculate the mesh bounds in world space.
        Bounds meshBounds = meshFilter.GetComponent<Renderer>().bounds;

        // Find the bottom of the collider.
        float colliderBottom = collider.bounds.min.y;

        // Calculate the required vertical adjustment.
        float offset = colliderBottom - meshBounds.min.y;

        // Move only the visual child.
        visual.position += Vector3.up * offset;
    }
}
