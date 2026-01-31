using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Room Database")]
public class RoomDatabase : ScriptableObject
{
    [SerializeField] private List<RoomConfig> rooms;

    private Dictionary<Room, RoomView> _lookup;

    public RoomView GetRoomPrefab(Room roomType)
    {
        _lookup ??= BuildLookup();

        if (_lookup.TryGetValue(roomType, out var prefab))
            return prefab;

        Debug.LogError($"Room not found: {roomType}");
        return null;
    }

    private Dictionary<Room, RoomView> BuildLookup()
    {
        var dict = new Dictionary<Room, RoomView>();

        foreach (var r in rooms)
        {
            if (!dict.ContainsKey(r.roomType))
                dict.Add(r.roomType, r.roomView);
        }

        return dict;
    }
}