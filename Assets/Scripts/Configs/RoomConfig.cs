using EditorAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Room Data")]
public class RoomData : ScriptableObject
{
    public string roomId;
    public Sprite background;
    public Sprite fullroom;
}