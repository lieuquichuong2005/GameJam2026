using UnityEngine;

public enum Room
{
    LivingRoom, 
    KitchenRoom,
    Attic, 
    Reading,
}

[CreateAssetMenu(menuName = "Game/Room Data")]
public class RoomConfig : ScriptableObject
{
    public Room roomType;
    public Sprite background;
    public Sprite fullroom;
    public GameObject roomPrefab;
    public RoomView roomView;
}