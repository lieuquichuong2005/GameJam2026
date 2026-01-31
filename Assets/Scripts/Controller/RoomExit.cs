using UnityEngine;

public class RoomExit : MonoBehaviour, IInteractable
{
    [SerializeField] private RoomView targetRoom;

    public Transform Transform { get; }
    public float InteractDistance { get; }

    public bool CanInteract()
    {
        throw new System.NotImplementedException();
    }

    public void Interact()
    {
        FindObjectOfType<LevelView>()
            .ChangeRoom(targetRoom);
    }
}