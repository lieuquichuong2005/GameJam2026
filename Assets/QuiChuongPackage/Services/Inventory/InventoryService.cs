using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class InventoryService
{
    private readonly List<InventoryItem> items = new();
    private ItemMergeDatabase mergeDatabase;

    public IReadOnlyList<InventoryItem> Items => items;
    public InventoryItem SelectedItem { get; private set; }

    // ===== EVENTS =====
    public event Action<InventoryItem> OnItemAdded;
    public event Action<InventoryItem> OnItemRemoved;
    public event Action<InventoryItem> OnSelected;
    public event Action OnDeselected;
    public event Action OnInventoryChanged;

    public void SetData(ItemMergeDatabase database)
    {
        Debug.Log("SetData");
        mergeDatabase = database;
        if (mergeDatabase == null)
        {
            Debug.LogError("No merge database");
        }
    }

    public void AddItem(InventoryItem item)
    {
        if (item == null || items.Contains(item)) return;
        items.Add(item);
        OnItemAdded?.Invoke(item);
        OnInventoryChanged?.Invoke();
    }

    public void RemoveItem(InventoryItem item)
    {
        if (!items.Remove(item)) return;
        OnItemRemoved?.Invoke(item);
        OnInventoryChanged?.Invoke();
    }

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

    public bool CanMerge(InventoryItem a, InventoryItem b)
    {
        Debug.Log($"Merge between {a} and {b}");
        if (mergeDatabase == null)
        {
            Debug.Log($"Null Database");
        }

        Debug.Log($"Can Merge State Is " + mergeDatabase.CanMerge(a, b));

        return mergeDatabase != null && mergeDatabase.CanMerge(a, b);
    }

    public InventoryItem Merge(InventoryItem a, InventoryItem b)
    {
        return mergeDatabase != null
            ? mergeDatabase.GetResult(a, b)
            : null;
    }
}