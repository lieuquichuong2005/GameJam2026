[System.Serializable]
public class GameData
{
    public int gold = 0;
    public int level = 1;
    public bool soundOn = true;
}

[System.Serializable]
public class SaveData
{
    public int version = 1;
    public GameData data = new();
}
