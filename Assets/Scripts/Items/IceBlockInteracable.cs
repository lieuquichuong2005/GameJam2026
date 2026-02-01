using System;
using UnityEngine;

public class IceBlockInteractable : InjectableMonoBehaviour, IInteractable
{
    [Inject] private AudioService _audioService;

    [Header("Interaction")] [SerializeField]
    private bool _requirePlayerNear = true;

    [SerializeField] private float _interactDistance = 0.3f;

    [Header("Ice")] [SerializeField] private int _breakClicksRequired = 3;
    [SerializeField] private GameObject _iceVisual;
    [SerializeField] private GameObject _iceItemPickup;
    [SerializeField] private GameObject _crack_01;
    [SerializeField] private GameObject _crack_02;

    private int clickCount = 0;
    private bool broken = false;

    public Transform Transform => transform;
    public bool RequirePlayerNear => _requirePlayerNear;
    public float InteractDistance => _interactDistance;

    public static Action SetCanInteractable;

    protected override void Awake()
    {
        base.Awake();
    }

    public bool CanInteract() => !broken;

    public void Interact()
    {
        if (broken) return;

        clickCount++;
        if (clickCount == 1)
        {
            _crack_01.SetActive(true);
            _audioService.Play(AudioId.IceBreak_01);
        }
        else if (clickCount == 2)
        {
            _crack_02.SetActive(true);
            _audioService.Play(AudioId.IceBreak_02);
        }

        Debug.Log($"[ICE] Hit {clickCount}/{_breakClicksRequired}");

        // TODO: play crack anim / sound

        if (clickCount >= _breakClicksRequired)
        {
            _audioService.Play(AudioId.IceBreak_03);
            BreakIce();
        }
    }

    private void BreakIce()
    {
        broken = true;

        _iceVisual.SetActive(false);
        _iceItemPickup.SetActive(true);

        SetCanInteractable?.Invoke();
        Debug.Log("[ICE] Ice broken!");
    }
}