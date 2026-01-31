using System;
using UnityEngine;

public static class LevelEventBus
{
    public static Action<string, Vector2> OnRequestChangeRoom;

    public static void RequestChangeRoom(string roomId, Vector2 spawnPos)
    {
        OnRequestChangeRoom?.Invoke(roomId, spawnPos);
    }
}