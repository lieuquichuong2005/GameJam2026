using System;
using EditorAttributes;
using UnityEngine;

public class FridgeDoorInteractable : InjectableMonoBehaviour, IInteractable
{
    [Inject] private AudioService _audioService;

    [SerializeField] private GameObject _doorClosed;
    [SerializeField] private GameObject _doorOpened;
    [Required] [SerializeField] private Collider2D _collider;

    private bool isOpened = false;
    private bool _isCanInteract = true;

    protected override void Awake()
    {
        base.Awake();
    }

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
        _audioService.Play(AudioId.OpenFridge);
        isOpened = true;

        _doorClosed.SetActive(false);
        _doorOpened.SetActive(true);

        _isCanInteract = false;
        Debug.Log("[FRIDGE] Door opened");
    }

    private void CloseDoor()
    {
        _audioService.Play(AudioId.CloseFridge);
        isOpened = false;
        _doorOpened.SetActive(false);
        _doorClosed.SetActive(true);

        _isCanInteract = true;
        Debug.Log("[FRIDGE] Door closed");
    }
}