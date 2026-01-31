public interface IItemReceiver
{
    bool CanUseItem(InventoryItem item);
    void UseItem(InventoryItem item);
}