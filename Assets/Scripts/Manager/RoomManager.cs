using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private Transform roomRoot;

    private RoomView currentRoom;

    public void EnterRoom(RoomView prefab, GameState state)
    {
        if (currentRoom != null)
        {
            currentRoom.OnExit();
            Destroy(currentRoom.gameObject);
        }

        currentRoom = Instantiate(prefab, roomRoot);
        currentRoom.transform.localPosition = Vector3.zero;

        currentRoom.OnEnter(state);
    }
}