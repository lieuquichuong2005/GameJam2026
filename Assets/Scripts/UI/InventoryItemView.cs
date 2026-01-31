using UnityEngine;
using UnityEngine.UI;

public class InventoryItemView : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Button button;
    [SerializeField] private GameObject selectedFrame;

    private InventoryItem data;
    private InventoryService inventory;
    private bool isEmpty = true;

    public void Init(InventoryService service)
    {
        inventory = service;
        button.onClick.AddListener(OnClick);

        inventory.OnSelected += HandleSelected;
        inventory.OnDeselected += HandleDeselected;

        ClearSlot();
    }

    public void SetItem(InventoryItem item)
    {
        if (item == null)
        {
            ClearSlot();
            return;
        }

        data = item;
        icon.sprite = item.icon;
        icon.enabled = true;
        icon.gameObject.SetActive(true);
        isEmpty = false;
    }

    public void ClearSlot()
    {
        data = null;
        icon.sprite = null;
        icon.enabled = false;
        icon.gameObject.SetActive(false);
        isEmpty = true;
        selectedFrame.SetActive(false);
    }

    public bool IsEmpty() => isEmpty;
    public InventoryItem GetItem() => data;

    public void HideIcon() => icon.enabled = false;
    public void ShowIcon() => icon.enabled = true;

    private void OnClick()
    {
        if (isEmpty) return;
        inventory.Select(data);
    }

    private void HandleSelected(InventoryItem selected)
    {
        selectedFrame.SetActive(selected == data);
    }

    private void HandleDeselected()
    {
        selectedFrame.SetActive(false);
    }

    private void OnDestroy()
    {
        if (inventory != null)
        {
            inventory.OnSelected -= HandleSelected;
            inventory.OnDeselected -= HandleDeselected;
        }

        button.onClick.RemoveAllListeners();
    }
}