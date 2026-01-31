using UnityEngine;

[CreateAssetMenu(menuName = "Config/Inventory/Item")]
public class InventoryItem : ScriptableObject
{
    public string itemId;
    public ItemType itemType;
    public Sprite icon;
    
    [Header("Inspect")] public Sprite inspectSprite;
    public bool canRotate;
}