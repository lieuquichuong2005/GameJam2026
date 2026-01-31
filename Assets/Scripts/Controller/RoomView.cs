using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomView : MonoBehaviour
{
    [SerializeField] private RoomConfig data;

    [Serializable]
    public class MaskView
    {
        public SpriteRenderer spriteRenderer;
        public Sprite normalSprite;
        public Sprite BloodSprite;
    }

    public List<GameObject> normalMask = new();
    public List<GameObject> bloodMask = new();
    public List<MaskView> maskViews = new();

    public RoomConfig Data => data;

    private void OnEnable()
    {
        MaskButton.IsShowingBloodMaskAction += UpdateMaskView;
    }

    private void OnDisable()
    {
        MaskButton.IsShowingBloodMaskAction -= UpdateMaskView;
    }

    public virtual void OnEnter(GameState state)
    {
        // đọc state → set hiển thị
    }

    public virtual void OnExit()
    {
        // cleanup nếu cần
    }

    public void UpdateMaskView(bool isBloodMaskOn)
    {
        foreach (var maskView in maskViews)
        {
            maskView.spriteRenderer.sprite = isBloodMaskOn ? maskView.BloodSprite : maskView.normalSprite;
        }

        foreach (var go in bloodMask)
        {
            go.SetActive(isBloodMaskOn);
        }

        foreach (var go in normalMask)
        {
            go.SetActive(!isBloodMaskOn);
        }
    }
}