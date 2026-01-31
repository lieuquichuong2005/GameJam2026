using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    [Header("Interaction")]
    [SerializeField] private bool _requirePlayerNear = true;
    [SerializeField] private float _interactDistance = 0.5f;

    [Header("Room")]
    [SerializeField] private string _targetRoomId;
    [SerializeField] private Vector2 _spawnPositionInTargetRoom;

    public Transform Transform => transform;
    public bool RequirePlayerNear => _requirePlayerNear;
    public float InteractDistance => _interactDistance;

    public bool CanInteract()
    {
        return !string.IsNullOrEmpty(_targetRoomId);
    }

    public void Interact()
    {
        Debug.Log($"[DOOR] Go to room: {_targetRoomId}");

        LevelEventBus.RequestChangeRoom(
            _targetRoomId,
            _spawnPositionInTargetRoom
        );
    }
}