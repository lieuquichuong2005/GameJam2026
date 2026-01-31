public interface IItemDropTarget
{
    bool CanAccept(InventoryItem item);
    void OnItemDropped(InventoryItem item);
}