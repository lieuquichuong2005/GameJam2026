using UnityEngine;

public class RoomView : MonoBehaviour
{
    [SerializeField] private RoomConfig data;

    public RoomConfig Data => data;

    public virtual void OnEnter(GameState state)
    {
        // đọc state → set hiển thị
    }

    public virtual void OnExit()
    {
        // cleanup nếu cần
    }
}