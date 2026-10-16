using System.IO;
using UnityEngine;

public static class SaveManager
{
    private static SaveData saveData = new SaveData();
    
    public static int HighScore => saveData.highScore;

    private static readonly string savePath = Path.Combine(Application.persistentDataPath, "savegame.json");

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        Load();
    }
    
    public static void TrySaveHighScore(int score)
    {
        if (score <= saveData.highScore)
            return;
        
        saveData.highScore = score;

        Save();
    }

    public static void Save()
    {
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(savePath, json);
    }

    public static void Load()
    {
        if (!File.Exists(savePath))
        {
            saveData = new SaveData();
            return;
        }
        
        string json = File.ReadAllText(savePath);
        saveData = JsonUtility.FromJson<SaveData>(json);
    }
}
