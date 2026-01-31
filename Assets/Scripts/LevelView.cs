using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Quản lí Logic. Không quản lí UI.
/// Cập nhật bằng LevelScene thông qua IEntity
/// </summary>
public class LevelView : InjectableMonoBehaviour, IEntity
{
    [Inject] private InventoryService inventoryService;

    [SerializeField] private PlayerController player;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private float snapToGroundDistance = 5f;

    private bool hasMoveTarget;
    private float moveTargetX;
    private IInteractable pendingInteractable;
    private IItemPickup pendingItemPickup;

    [SerializeField] private RoomDatabase roomDatabase;
    [SerializeField] private RoomManager roomManager;
    [SerializeField] private GameState gameState;

    protected override void Awake()
    {
        base.Awake();
        if (inventoryService == null)
        {
            inventoryService = new InventoryService();
            Services.Register(inventoryService);
        }
    }

    public async UniTask PressOnPosition(Vector2 worldPos)
    {
        var hit = Physics2D.Raycast(worldPos, Vector2.zero);

        if (inventoryService != null &&
            inventoryService.SelectedItem != null &&
            hit.collider != null &&
            hit.collider.TryGetComponent<IItemReceiver>(out var receiver))
        {
            receiver.UseItem(inventoryService.SelectedItem);
            inventoryService.Select(null);
            return;
        }

        if (hit.collider != null &&
            hit.collider.TryGetComponent<IItemPickup>(out var itemPickup))
        {
            float itemX = itemPickup.Transform.position.x;

            if (player.ReachedX(itemX))
            {
                HandleItemPickup(itemPickup);
            }
            else
            {
                pendingItemPickup = itemPickup;
                pendingInteractable = null;

                moveTargetX = itemX;
                hasMoveTarget = true;

                player.MoveToX(moveTargetX);
            }

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

    public void ChangeRoom(RoomView roomPrefab)
    {
        roomManager.EnterRoom(roomPrefab, gameState);
    }

    public void EnterRoom(string roomId)
    {
        var prefab = roomDatabase.GetRoomPrefab(roomId);
        roomManager.EnterRoom(prefab, gameState);
        gameState.currentRoomId = roomId;
    }

    /// <summary>
    /// Xử lý nhặt item - KHÔNG CẦN ĐI LẠI
    /// </summary>
    private async void HandleItemPickup(IItemPickup itemPickup)
    {
        var item = itemPickup.GetItem();
        if (item == null) return;

        itemPickup.SetVisible(false);
        itemPickup.SetInteractable(false);

        await FindObjectOfType<LevelScene>()
            .PlayPickupItemEffect(item, itemPickup.Transform.position);

        Destroy(itemPickup.Transform.gameObject);
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

            if (pendingItemPickup != null)
            {
                HandleItemPickup(pendingItemPickup);
                pendingItemPickup = null;
                return;
            }

            if (pendingInteractable != null)
            {
                pendingInteractable.Interact();
                pendingInteractable = null;
            }
        }
    }
}