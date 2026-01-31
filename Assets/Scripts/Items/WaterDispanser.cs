using EditorAttributes;
using UnityEngine;

public class WaterDispenser : InjectableMonoBehaviour, IItemDropTarget
{
    [SerializeField] private ItemType _requiredItemType = ItemType.Glass;
    [SerializeField] private InventoryItem _filledCup;
    [SerializeField] private SpriteRenderer _glassSprite;

    [Required] [SerializeField]
    private Sprite _fullWaterSprite, _outOfWaterSprite, _blackWaterSprite, _bloodWaterSprite;

    private InventoryService inventory;

    protected override void Awake()
    {
        base.Awake();

        if (inventory == null)
        {
            inventory = Services.Get<InventoryService>();
        }
    }

    public bool CanAccept(InventoryItem item)
    {
        return item != null && item.itemType == _requiredItemType;
    }

    public void OnItemDropped(InventoryItem item)
    {
        Debug.Log("[WATER] Filling cup");

        _glassSprite.sprite = _outOfWaterSprite;

        inventory.RemoveItem(item);
        inventory.AddItem(_filledCup);
    }
}