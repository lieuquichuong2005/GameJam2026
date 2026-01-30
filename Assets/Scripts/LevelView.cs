using UnityEngine;

/// <summary>
/// Quản lí Logic. Không quản lí UI.
/// Cập nhật bằng LevelScene thông qua IEntity
/// </summary>
public class LevelView : MonoBehaviour, IEntity
{
    [SerializeField] private PlayerController player;

    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private float snapToGroundDistance = 5f;

    private bool hasMoveTarget;
    private float moveTargetX;

    private IInteractable pendingInteractable;

    public void OnClickWorld(Vector2 worldPos)
    {
        Debug.Log($"[LEVEL] Click at {worldPos}");

        hasMoveTarget = false;
        pendingInteractable = null;

        var hit = Physics2D.Raycast(worldPos, Vector2.zero);
        if (hit.collider != null &&
            hit.collider.TryGetComponent<IInteractable>(out var interactable))
        {
            HandleInteractableClick(interactable);
            return;
        }

        if (!WalkableUtility.TryGetWalkablePoint(
                worldPos,
                player.transform.position,
                out Vector2 walkablePos,
                snapToGroundDistance,
                groundMask))
        {
            Debug.Log("[LEVEL] Invalid walkable point");
            return;
        }

        if (!WalkableUtility.IsPathClear(
                player.transform.position,
                walkablePos,
                obstacleMask))
        {
            Debug.Log("[LEVEL] Path blocked");
            return;
        }

        moveTargetX = walkablePos.x;
        hasMoveTarget = true;

        Debug.Log($"[LEVEL] Move target X set: {moveTargetX}");
        player.MoveToX(moveTargetX);
    }

    private void HandleInteractableClick(IInteractable interactable)
    {
        Debug.Log($"[LEVEL] Clicked interactable: {interactable}");

        if (!interactable.CanInteract())
        {
            Debug.Log("[LEVEL] Interactable cannot interact now");
            return;
        }

        float interactX = interactable.Transform.position.x;

        if (player.ReachedX(interactX))
        {
            Debug.Log("[LEVEL] Interact immediately");
            interactable.Interact();
        }
        else
        {
            Debug.Log("[LEVEL] Move to interactable first");
            pendingInteractable = interactable;

            moveTargetX = interactX;
            hasMoveTarget = true;

            player.MoveToX(moveTargetX);
        }
    }

    public void OnUpdate(float deltaTime)
    {
        if (!hasMoveTarget) return;

        if (player.ReachedX(moveTargetX))
        {
            Debug.Log("[LEVEL] Reached move target");
            hasMoveTarget = false;

            if (pendingInteractable != null)
            {
                Debug.Log("[LEVEL] Reached interactable → interact");
                pendingInteractable.Interact();
                pendingInteractable = null;
            }
        }
    }
}