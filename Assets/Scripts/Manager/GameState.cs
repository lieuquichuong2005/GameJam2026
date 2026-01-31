using System.Collections.Generic;

[System.Serializable]
public class GameState
{
    public string currentRoomId;
    public Dictionary<string, bool> puzzleFlags = new();
}