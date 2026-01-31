using UnityEngine;

/// <summary>
/// Template cho các item khác: Key, Coin, Note, etc.
/// Copy file này và đổi tên class
/// </summary>
public class Item : InjectableMonoBehaviour, IItemPickup
{
    [SerializeField] private InventoryItem itemToGive;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Inject] private InventoryService inventoryService;

    private bool collected = false;

    protected override void Awake()
    {
        base.Awake();

        if (itemToGive != null && spriteRenderer != null && spriteRenderer.sprite == null)
        {
            spriteRenderer.sprite = itemToGive.icon;
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

        Debug.Log($"[ItemPickup] Picked up: {itemToGive.itemId}");

        inventoryService.AddItem(itemToGive);
        collected = true;

        Destroy(gameObject, 0.2f);
    }

    public InventoryItem GetItem() => itemToGive;
}