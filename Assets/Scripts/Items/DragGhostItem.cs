using UnityEngine;
using UnityEngine.UI;

public class DragItemGhost : MonoBehaviour
{
    [SerializeField] private Image icon;

    public void SetSprite(Sprite sprite)
    {
        icon.sprite = sprite;
    }

    public void Follow(Vector2 screenPos)
    {
        transform.position = screenPos;
    }
}