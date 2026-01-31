using System;
using EditorAttributes;
using UnityEngine;

public class SafeInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private SafeController controller;
    [SerializeField] private SpriteRenderer _safeSpriteRenderer;
    [SerializeField] private Collider2D _collider;
    [Required] [SerializeField] private Sprite _safeCloseSprite, _safeOpenSprite;
    [SerializeField] private GameObject _noteObject;

    public Transform Transform => transform;
    public float InteractDistance => 0.5f;
    public bool RequirePlayerNear { get; }

    private void OnEnable()
    {
        controller.IsUnlocked += ChangeSprite;
    }

    private void OnDisable()
    {
        controller.IsUnlocked -= ChangeSprite;
    }

    private void ChangeSprite(bool isUnlocked)
    {
        if (isUnlocked)
        {
            _safeSpriteRenderer.sprite = isUnlocked ? _safeOpenSprite : _safeCloseSprite;
        }

        _collider.enabled = !isUnlocked;
        _noteObject.SetActive(isUnlocked);
    }

    public bool CanInteract()
    {
        return !controller.IsOpened;
    }

    public void Interact()
    {
        controller.OpenInput();
    }
}