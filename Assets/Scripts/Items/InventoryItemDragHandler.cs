using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class InventoryItemDragHandler : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private InventoryItemView view;
    [SerializeField] private DragItemGhost ghostPrefab;

    private DragItemGhost ghost;
    private Canvas canvas;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (view.IsEmpty()) return;

        view.HideIcon();

        ghost = Instantiate(ghostPrefab, canvas.transform);
        ghost.SetSprite(view.GetItem().icon);
        ghost.Follow(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (ghost == null) return;
        ghost.Follow(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (ghost == null) return;

        bool success = TryDrop(eventData);

        if (!success)
            view.ShowIcon();
        else
            view.ClearSlot();

        if (ghost != null)
            Destroy(ghost.gameObject);
    }

    private bool TryDrop(PointerEventData eventData)
    {
        var uiResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, uiResults);

        foreach (var r in uiResults)
        {
            if (r.gameObject.TryGetComponent<IItemDropTarget>(out var uiTarget))
            {
                return TryAccept(uiTarget);
            }
        }

        var cam = canvas.worldCamera != null
            ? canvas.worldCamera
            : Camera.main;

        Vector2 worldPos = cam.ScreenToWorldPoint(eventData.position);

        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

        if (hit.collider != null &&
            hit.collider.TryGetComponent<IItemDropTarget>(out var worldTarget))
        {
            return TryAccept(worldTarget);
        }

        return false;
    }

    private bool TryAccept(IItemDropTarget target)
    {
        var item = view.GetItem();
        if (!target.CanAccept(item))
            return false;

        target.OnItemDropped(item);
        return true;
    }
}