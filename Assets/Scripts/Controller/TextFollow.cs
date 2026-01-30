using UnityEngine;
using TMPro;

public class TextFollow : MonoBehaviour
{
    [SerializeField] private Transform target;     // character transform
    [SerializeField] private Vector3 worldOffset = new Vector3(0, 1.6f, 0);
    [SerializeField] private RectTransform uiElement; // the TMP RectTransform
    [SerializeField] private Canvas canvas;

    private Camera cam;

    void Awake()
    {
        cam = Camera.main;
        if (!uiElement) uiElement = (RectTransform)transform;
        if (!canvas) canvas = GetComponentInParent<Canvas>();
    }

    void LateUpdate()
    {
        if (!target || !cam || !canvas) return;

        Vector3 worldPos = target.position + worldOffset;
        Vector3 screenPos = cam.WorldToScreenPoint(worldPos);

        // If behind camera, hide
        bool behind = screenPos.z < 0f;
        uiElement.gameObject.SetActive(!behind);
        if (behind) return;

        // Convert screen position to canvas position
        RectTransform canvasRect = (RectTransform)canvas.transform;
        Vector2 anchoredPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPos,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : cam,
            out anchoredPos
        );

        uiElement.anchoredPosition = anchoredPos;
    }

    public void SetTarget(Transform newTarget) => target = newTarget;
}
