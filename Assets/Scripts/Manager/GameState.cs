using System.Collections.Generic;

[System.Serializable]
public class GameState
{
    public Room currentRoomType;
    public Dictionary<string, bool> puzzleFlags = new();
}