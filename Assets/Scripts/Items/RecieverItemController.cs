    using System;
using System.Collections.Generic;
using UnityEngine;

public class StoveReceiver : MonoBehaviour, IItemDropTarget
{
    [Serializable]
    public class VisualObject
    {
        public ItemType ItemType;
        public GameObject Visual;
    }

    [Header("Recipe")] [SerializeField] private StoveRecipe recipe;

    [Header("Visual")] [SerializeField] private List<VisualObject> ingredientVisuals;
    [SerializeField] private GameObject cookedVisual;

    private HashSet<ItemType> addedItems = new();
    private bool isCooked;
    Dictionary<ItemType, GameObject> ingredients = new Dictionary<ItemType, GameObject>();

    private void Awake()
    {
        ingredients = new Dictionary<ItemType, GameObject>();
        foreach (var vo in ingredientVisuals)
        {
            ingredients.Add(vo.ItemType, vo.Visual);
        }
    }

    public bool CanAccept(InventoryItem item)
    {
        if (isCooked) return false;

        return recipe.requiredItemIds.Contains(item.itemType)
               && !addedItems.Contains(item.itemType);
    }

    public void OnItemDropped(InventoryItem item)
    {
        if (!CanAccept(item)) return;

        addedItems.Add(item.itemType);
        ShowIngredientVisual(item.itemType);

        Debug.Log($"[STOVE] Added {item.itemId} ({addedItems.Count}/{recipe.requiredItemIds.Count})");

        if (addedItems.Count >= recipe.requiredItemIds.Count)
        {
            Cook();
        }
    }

    private void ShowIngredientVisual(ItemType itemType)
    {
        var vo = ingredients[itemType];
        vo.SetActive(true);
    }

    private void Cook()
    {
        isCooked = true;
        cookedVisual.SetActive(true);

        Debug.Log("[STOVE] Cooking completed!");

        // TODO:
        // QuestService.CompleteStep(...)
        // SoundManager.Play("cook")
        // Animation
    }
}