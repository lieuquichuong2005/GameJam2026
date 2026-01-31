using System;
using EditorAttributes;
using UnityEngine;

public class FridgeDoorInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _doorClosed;
    [SerializeField] private GameObject _doorOpened;
    [Required] [SerializeField] private Collider2D _collider;

    private bool isOpened = false;
    private bool _isCanInteract = true;

    private void OnEnable()
    {
        IceBlockInteractable.SetCanInteractable += SetInteracable;
    }

    private void OnDisable()
    {
        IceBlockInteractable.SetCanInteractable -= SetInteracable;
    }

    private void SetInteracable()
    {
        _isCanInteract = true;
    }

    public Transform Transform => transform;
    public float InteractDistance { get; }
    public bool RequirePlayerNear { get; }

    public bool CanInteract() => !isOpened;

    public void Interact()
    {
        if (!_isCanInteract)
        {
            return;
        }

        if (isOpened)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        isOpened = true;

        _doorClosed.SetActive(false);
        _doorOpened.SetActive(true);

        _isCanInteract = false;
        Debug.Log("[FRIDGE] Door opened");
    }

    private void CloseDoor()
    {
        isOpened = false;
        _doorOpened.SetActive(false);
        _doorClosed.SetActive(true);

        _isCanInteract = true;
        Debug.Log("[FRIDGE] Door closed");
    }
}