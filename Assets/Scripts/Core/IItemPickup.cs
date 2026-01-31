/// <summary>
/// Interface đánh dấu object là item có thể nhặt ngay khi click
/// Không cần đi lại gần
/// </summary>
public interface IItemPickup : IInteractable
{
    InventoryItem GetItem();
    void SetVisible(bool visible);
    void SetInteractable(bool value);
}