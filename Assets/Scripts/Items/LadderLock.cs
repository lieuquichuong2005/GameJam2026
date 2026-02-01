using System.Collections.Generic;
using UnityEngine;

public class LadderLock : MonoBehaviour, IItemDropTarget
{
    [Header("Recipe")][SerializeField] private StoveRecipe recipe;
    private bool isLocked;
    private HashSet<ItemType> addedItems = new();
    private InventoryService _inventoryService;
    private AudioSource Audio;
    private Collider2D myCol;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Audio = GetComponent<AudioSource>();
        myCol = GetComponent<Collider2D>();
    }
    public bool CanAccept(InventoryItem item)
    {

        return recipe.requiredItemIds.Contains(item.itemType)
               && !addedItems.Contains(item.itemType);
    }

    // Update is called once per frame
    public void OnItemDropped(InventoryItem item)
    {
        if (!CanAccept(item)) return;

        addedItems.Add(item.itemType);
        
        _inventoryService.RemoveItem(item);

        if (addedItems.Count >= recipe.requiredItemIds.Count)
        {
            if (Audio != null) Audio.Play();
            myCol.isTrigger = true;

        }
    }
}
