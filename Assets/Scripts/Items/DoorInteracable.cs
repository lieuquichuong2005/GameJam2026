using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    [Header("Interaction")] [SerializeField]
    private bool _requirePlayerNear = true;

    [SerializeField] private float _interactDistance = 0.5f;

    [Header("Room")] [SerializeField] private Room _targetRoomType;
    [SerializeField] private Vector2 _spawnPositionInTargetRoom;

    public Transform Transform => transform;
    public bool RequirePlayerNear => _requirePlayerNear;
    public float InteractDistance => _interactDistance;

    public bool CanInteract()
    {
        return true;
    }

    public void Interact()
    {
        Debug.Log($"[DOOR] Go to room: {_targetRoomType}");

        LevelEventBus.RequestChangeRoom(
            _targetRoomType,
            _spawnPositionInTargetRoom
        );
    }
}