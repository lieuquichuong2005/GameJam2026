using UnityEngine;

[CreateAssetMenu(menuName = "Config/Inventory/Item")]
public class InventoryItem : ScriptableObject
{
    public ItemType itemType;
    public Sprite icon;
    
    [Header("Inspect")] public Sprite inspectSprite;
    public bool canRotate;
}