using System;
using UnityEngine;

public static class LevelEventBus
{
    public static Action<Room, Vector2> OnRequestChangeRoom;

    public static void RequestChangeRoom(Room roomType, Vector2 spawnPos)
    {
        OnRequestChangeRoom?.Invoke(roomType, spawnPos);
    }
}