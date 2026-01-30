using System.IO;
using UnityEngine;

public class JsonDataService : IDataService
{
    private const string FILE_NAME = "game_data.json";
    private readonly string filePath;

    public GameData Data { get; private set; }

    public JsonDataService()
    {
        filePath = Path.Combine(Application.persistentDataPath, FILE_NAME);
    }

    public void Load()
    {
        if (!File.Exists(filePath))
        {
            Data = new GameData();
            Save();
            return;
        }

        var json = File.ReadAllText(filePath);
        var save = JsonUtility.FromJson<SaveData>(json);

        Data = save?.data ?? new GameData();
    }

    public void Save()
    {
        var save = new SaveData
        {
            data = Data
        };

        var json = JsonUtility.ToJson(save, prettyPrint: true);
        File.WriteAllText(filePath, json);
    }

    public void Reset()
    {
        Data = new GameData();
        Save();
    }
}