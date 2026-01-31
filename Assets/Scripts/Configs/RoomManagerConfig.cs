using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Room Database")]
public class RoomDatabase : ScriptableObject
{
    [SerializeField] private List<RoomConfig> rooms;

    private Dictionary<string, RoomView> _lookup;

    public RoomView GetRoomPrefab(string roomId)
    {
        _lookup ??= BuildLookup();

        if (_lookup.TryGetValue(roomId, out var prefab))
            return prefab;

        Debug.LogError($"Room not found: {roomId}");
        return null;
    }

    private Dictionary<string, RoomView> BuildLookup()
    {
        var dict = new Dictionary<string, RoomView>();

        foreach (var r in rooms)
        {
            if (!dict.ContainsKey(r.roomId))
                dict.Add(r.roomId, r.roomView);
        }

        return dict;
    }
}