using UnityEngine;

[CreateAssetMenu(menuName = "Game/Room Data")]
public class RoomConfig : ScriptableObject
{
    public string roomId;
    public Sprite background;
    public Sprite fullroom;
    public GameObject roomPrefab;
    public RoomView roomView;
}