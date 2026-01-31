using EditorAttributes;
using UnityEngine;

/// <summary>
/// Template cho các item khác: Key, Coin, Note, etc.
/// Copy file này và đổi tên class
/// </summary>
public enum ItemType
{
    None,
    Salt,
    Knife,
    Butter,
    Flour,
    Cake,
    Diary,
    AppleJam,
    Iced,
    Water,
    Glass,
    GlassOfIced,
    GlassOfWater,
}

public class Item : InjectableMonoBehaviour, IItemPickup
{
    [SerializeField] private ItemType _itemType;
    [Required] [SerializeField] private InventoryItem _itemToGive;
    [Required] [SerializeField] private SpriteRenderer _spriteRenderer;
    [Required] [SerializeField] private Collider2D _collider2d;

    [Inject] private InventoryService inventoryService;

    [Header("Interaction")] [SerializeField]
    private bool _requirePlayerNear = true;

    [SerializeField] private float _interactDistance = 0.3f;

    [SerializeField] private int _healthPoints = 1;

    private bool collected = false;

    protected override void Awake()
    {
        base.Awake();

        if (_itemToGive != null && _spriteRenderer != null && _spriteRenderer.sprite == null)
        {
            _spriteRenderer.sprite = _itemToGive.icon;
        }
    }

    public Transform Transform => transform;

    /// ⭐ QUAN TRỌNG
    public bool RequirePlayerNear => _requirePlayerNear;

    public float InteractDistance => _interactDistance;

    public ItemType ItemType => _itemType;

    public bool CanInteract()
    {
        return !collected && _itemToGive != null && inventoryService != null;
    }

    public void Interact()
    {
        if (!CanInteract())
            return;

        _healthPoints--;

        if (_healthPoints > 0)
            return;

        Debug.Log($"[ItemPickup] Picked up: {_itemToGive.itemId}");

        inventoryService.AddItem(_itemToGive);
        collected = true;

        Destroy(gameObject, 0.2f);
    }

    public InventoryItem GetItem() => _itemToGive;

    public void SetVisible(bool visible)
    {
        _spriteRenderer.enabled = visible;
    }

    public void SetInteractable(bool value)
    {
        _collider2d.enabled = value;
    }
}