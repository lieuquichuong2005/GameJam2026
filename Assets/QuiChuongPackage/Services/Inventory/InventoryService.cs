using System;
using System.Collections.Generic;

public class InventoryService
{
    private readonly List<InventoryItem> items = new();

    public IReadOnlyList<InventoryItem> Items => items;
    public InventoryItem SelectedItem { get; private set; }

    // ===== EVENTS =====
    public event Action<InventoryItem> OnItemAdded;
    public event Action<InventoryItem> OnItemRemoved;
    public event Action<InventoryItem> OnSelected;
    public event Action OnDeselected;

    // ===== ITEMS =====
    public void AddItem(InventoryItem item)
    {
        if (item == null || items.Contains(item))
            return;

        items.Add(item);
        OnItemAdded?.Invoke(item);
    }

    public void RemoveItem(InventoryItem item)
    {
        if (!items.Remove(item))
            return;

        if (SelectedItem == item)
            Deselect();

        OnItemRemoved?.Invoke(item);
    }

    // ===== SELECTION =====
    public void Select(InventoryItem item)
    {
        if (item == null)
        {
            Deselect();
            return;
        }

        if (!items.Contains(item))
            return;

        if (SelectedItem == item)
        {
            Deselect();
            return;
        }

        SelectedItem = item;
        OnSelected?.Invoke(item);
    }

    public void Deselect()
    {
        if (SelectedItem == null)
            return;

        SelectedItem = null;
        OnDeselected?.Invoke();
    }
}