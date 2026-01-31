using UnityEngine;
using UnityEngine.UI;

public class InspectItemView : MonoBehaviour, IEntity
{
    [SerializeField] private Image itemImage;
    [SerializeField] private CanvasGroup canvasGroup;

    private bool dragging;
    private float rotation;

    public void Show(InventoryItem item)
    {
        itemImage.sprite = item.inspectSprite;
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }

    public void Hide()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        itemImage.transform.rotation = Quaternion.identity;
    }

    public void OnUpdate(float deltaTime)
    {
        if (!canvasGroup.blocksRaycasts) return;

        if (Input.GetMouseButtonDown(0))
            dragging = true;

        if (Input.GetMouseButtonUp(0))
            dragging = false;

        if (dragging)
        {
            float delta = Input.GetAxis("Mouse X");
            rotation += delta * 5f;
            itemImage.transform.rotation = Quaternion.Euler(0, 0, rotation);
        }
    }
}