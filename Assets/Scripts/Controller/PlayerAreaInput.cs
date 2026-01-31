using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayAreaInput :
    MonoBehaviour,
    IPointerDownHandler
{
    public static Action<Vector2> OnClick;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnClick?.Invoke(eventData.position);
    }
}