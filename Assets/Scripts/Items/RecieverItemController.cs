using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using EditorAttributes;
using UnityEngine;

public class StoveReceiver : InjectableMonoBehaviour, IItemDropTarget
{
    [Serializable]
    public class VisualObject
    {
        public ItemType ItemType;
        public GameObject Visual;
    }

    [Inject] private AudioService _audioService;

    [Header("Recipe")] [SerializeField] private StoveRecipe recipe;

    [Header("Visual")] [SerializeField] private List<VisualObject> ingredientVisuals;
    [SerializeField] private GameObject cookedVisual;
    [Required] [SerializeField] private GameObject _pieCake;
    private InventoryService _inventoryService;
    private HashSet<ItemType> addedItems = new();
    private bool isCooked;
    Dictionary<ItemType, GameObject> ingredients = new Dictionary<ItemType, GameObject>();

    protected override void Awake()
    {
        base.Awake();
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
        _inventoryService.RemoveItem(item);

        if (addedItems.Count >= recipe.requiredItemIds.Count)
        {
            _ = Cook();
        }
    }

    private void ShowIngredientVisual(ItemType itemType)
    {
        var vo = ingredients[itemType];
        vo.SetActive(true);
    }

    private async UniTask Cook()
    {
        foreach (var vo in ingredientVisuals)
        {
            vo.Visual.SetActive(false);
        }

        _audioService.Play(AudioId.CookCake);

        isCooked = true;
        cookedVisual.SetActive(true);

        await UniTask.Delay(8000);

        cookedVisual.SetActive(false);
        _pieCake.SetActive(true);
    }
}