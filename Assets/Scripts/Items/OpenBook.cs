using UnityEngine;
using UnityEngine.EventSystems;

public class OpenBook : MonoBehaviour, IPointerDownHandler
{
    public GameObject UI;
    public void OnPointerDown(PointerEventData eventData)
    {
        UI.SetActive(true);
    }
}
