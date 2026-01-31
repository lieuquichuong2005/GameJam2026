using UnityEngine;

/// <summary>
/// Item pickup - Nhặt ngay khi click, không cần đi lại gần
/// </summary>
public class Battery : InjectableMonoBehaviour, IItemPickup
{
    [SerializeField] private InventoryItem itemToGive;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Inject] private InventoryService inventoryService;
    
    private bool collected = false;

    protected override void Awake()
    {
        base.Awake();
        
        if (inventoryService == null)
        {
            Debug.LogError("[Battery] InventoryService not injected!");
        }
    }

    public Transform Transform => transform;
    public float InteractDistance => 0f;

    public bool CanInteract()
    {
        return !collected && itemToGive != null && inventoryService != null;
    }

    public void Interact()
    {
        if (!CanInteract())
            return;

        Debug.Log($"[Battery] Picked up item: {itemToGive.itemId}");
        inventoryService.AddItem(itemToGive);

        collected = true;
        
        // Hiệu ứng biến mất (tùy chọn)
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }
        
        Destroy(gameObject, 0.1f); // Delay nhỏ để animation kịp chạy nếu có
    }

    public InventoryItem GetItem() => itemToGive;
}