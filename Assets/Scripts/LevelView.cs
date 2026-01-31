using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Quản lí Logic. Không quản lí UI.
/// Cập nhật bằng LevelScene thông qua IEntity
/// </summary>
public class LevelView : InjectableMonoBehaviour, IEntity
{
    [Inject] private InventoryService inventory;

    [SerializeField] private PlayerController player;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private float snapToGroundDistance = 5f;

    private bool hasMoveTarget;
    private float moveTargetX;
    private IInteractable pendingInteractable;

    protected override void Awake()
    {
        // base.Awake(); // ✅ BẮT BUỘC để injection hoạt động
        var inventoryService = new InventoryService();
        Services.Register(inventoryService);
        inventory = inventoryService;
    }

    public async UniTask PressOnPosition(Vector2 worldPos)
    {
        Debug.Log($"[LEVEL] Click at {worldPos}");

        var hit = Physics2D.Raycast(worldPos, Vector2.zero);

        if (inventory != null &&
            inventory.SelectedItem != null &&
            hit.collider != null &&
            hit.collider.TryGetComponent<IItemReceiver>(out var receiver))
        {
            receiver.UseItem(inventory.SelectedItem);
            inventory.Select(null); // deselect
            return;
        }

        if (hit.collider != null &&
            hit.collider.TryGetComponent<IItemPickup>(out var itemPickup))
        {
            HandleItemPickup(itemPickup);
            return;
        }

        if (hit.collider != null &&
            hit.collider.TryGetComponent<IInteractable>(out var interactable))
        {
            HandleInteractableClick(interactable);
            return;
        }

        hasMoveTarget = false;
        pendingInteractable = null;

        if (!WalkableUtility.TryGetWalkablePoint(
                worldPos,
                player.transform.position,
                out Vector2 walkablePos,
                snapToGroundDistance,
                groundMask))
            return;

        if (!WalkableUtility.IsPathClear(
                player.transform.position,
                walkablePos,
                obstacleMask))
            return;

        moveTargetX = walkablePos.x;
        hasMoveTarget = true;
        player.MoveToX(moveTargetX);
    }

    /// <summary>
    /// Xử lý nhặt item - KHÔNG CẦN ĐI LẠI
    /// </summary>
    private void HandleItemPickup(IItemPickup itemPickup)
    {
        Debug.Log($"[LEVEL] Clicked item pickup: {itemPickup.GetItem()?.itemId}");

        if (!itemPickup.CanInteract())
        {
            Debug.Log("[LEVEL] Cannot pickup this item");
            return;
        }

        // Nhặt ngay lập tức
        itemPickup.Interact();
    }

    /// <summary>
    /// Xử lý tương tác với object - CẦN ĐI LẠI GẦN
    /// </summary>
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