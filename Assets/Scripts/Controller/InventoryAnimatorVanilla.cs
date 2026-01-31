using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using EditorAttributes;

/// <summary>
/// Animate inventory slots - UniTask version
/// </summary>
public class InventoryAnimatorUniTask : MonoBehaviour
{
    [Header("References")] [SerializeField] [Required]
    private Button backpackButton;

    [Required] [SerializeField] private RectTransform _backpackIcon;
    [Required] [SerializeField] private Image _backpack;
    [Required] [SerializeField] private RectTransform _slotsContainer;
    [Required] [SerializeField] private List<RectTransform> _slots = new();

    [Header("Animation Settings")] [SerializeField]
    private float animationDuration = 0.3f;

    [SerializeField] private int slotDelay = 50;

    [SerializeField] private AnimationCurve easeCurve =
        AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Positions")] [SerializeField] private Vector2 closedPosition = Vector2.zero;
    [SerializeField] private float slotSpacing = 100f;

    [Header("Visual Effects")] [SerializeField]
    private bool useRotation = false;

    [SerializeField] private Sprite[] _backpackSprites;

    [SerializeField] private float rotationAmount = 360f;

    private bool isOpen;
    private bool isAnimating;
    private CancellationTokenSource animationCTS;
    public bool IsOpen => isOpen;
    public bool IsAnimating => isAnimating;
    private List<Vector2> cachedOpenPositions = new List<Vector2>();
    private bool layoutInitialized = false;

    private void Start()
    {
        backpackButton?.onClick.AddListener(ToggleInventory);
        InitializeClosedState();
    }

    private void InitializeClosedState()
    {
        if (_slots.Count == 0 && _slotsContainer != null)
        {
            foreach (Transform child in _slotsContainer)
            {
                var rect = child.GetComponent<RectTransform>();
                if (rect != null)
                {
                    _slots.Add(rect);
                }
            }
        }

        if (_slotsContainer != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(_slotsContainer);
            Canvas.ForceUpdateCanvases();
        }

        cachedOpenPositions.Clear();
        foreach (var slot in _slots)
        {
            cachedOpenPositions.Add(slot.anchoredPosition);
        }

        foreach (var slot in _slots)
        {
            slot.anchoredPosition = closedPosition;
            slot.localScale = Vector3.zero;
            if (useRotation)
            {
                slot.localRotation = Quaternion.identity;
            }
        }

        layoutInitialized = true;
    }

    public void ToggleInventory()
    {
        if (isAnimating) return;

        if (isOpen) CloseInventory();
        else OpenInventory();
    }

    public void OpenInventory()
    {
        if (isAnimating || isOpen) return;
        RunAnimation(AnimateOpenAsync);
    }

    public void CloseInventory()
    {
        if (isAnimating || !isOpen) return;
        RunAnimation(AnimateCloseAsync);
    }

    private void RunAnimation(
        System.Func<CancellationToken, UniTask> animation)
    {
        animationCTS?.Cancel();
        animationCTS = new CancellationTokenSource();
        animation(animationCTS.Token).Forget();
    }

    private async UniTask AnimateOpenAsync(CancellationToken token)
    {
        isAnimating = true;
        isOpen = true;
        _backpack.sprite = isOpen ? _backpackSprites[0] : _backpackSprites[1];

        if (!layoutInitialized)
        {
            InitializeClosedState();
        }

        if (_backpackIcon != null)
        {
            AnimateScaleAsync(
                _backpackIcon,
                Vector3.one,
                Vector3.one * 1.1f,
                0.1f,
                token
            ).Forget();
        }

        for (int i = 0; i < _slots.Count; i++)
        {
            RectTransform slot = _slots[i];
            Vector2 targetPos =
                closedPosition + Vector2.right * (i + 1) * slotSpacing;

            AnimateSlotOpenAsync(
                slot,
                targetPos,
                i * slotDelay,
                token
            ).Forget();
        }

        await UniTask.Delay(
            _slots.Count * slotDelay + Mathf.RoundToInt(animationDuration * 1000),
            cancellationToken: token
        );

        isAnimating = false;
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)_slotsContainer.transform);
    }

    private async UniTask AnimateCloseAsync(CancellationToken token)
    {
        isAnimating = true;
        isOpen = false;

        if (_backpackIcon != null)
        {
            AnimateScaleAsync(
                _backpackIcon,
                Vector3.one,
                Vector3.one * 0.9f,
                0.1f,
                token
            ).Forget();
        }

        for (int i = _slots.Count - 1; i >= 0; i--)
        {
            AnimateSlotCloseAsync(
                _slots[i],
                (_slots.Count - 1 - i) * slotDelay,
                token
            ).Forget();
        }

        await UniTask.Delay(
            _slots.Count * slotDelay + Mathf.RoundToInt(animationDuration * 1000),
            cancellationToken: token
        );

        isAnimating = false;

        _backpack.sprite = isOpen ? _backpackSprites[0] : _backpackSprites[1];
    }

    private async UniTask AnimateSlotOpenAsync(
        RectTransform slot,
        Vector2 targetPos,
        int delayMs,
        CancellationToken token)
    {
        await UniTask.Delay(delayMs, cancellationToken: token);

        int slotIndex = _slots.IndexOf(slot);
        if (slotIndex >= 0 && slotIndex < cachedOpenPositions.Count)
        {
            targetPos = cachedOpenPositions[slotIndex];
        }

        Vector2 startPos = slot.anchoredPosition;
        Vector3 startScale = slot.localScale;
        Quaternion startRot = slot.localRotation;
        Quaternion targetRot = useRotation ? Quaternion.Euler(0, 0, rotationAmount) : startRot;

        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            token.ThrowIfCancellationRequested();

            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / animationDuration);
            float curved = easeCurve.Evaluate(t);

            slot.anchoredPosition = Vector2.Lerp(startPos, targetPos, curved);
            slot.localScale = Vector3.Lerp(startScale, Vector3.one, curved);

            if (useRotation)
            {
                slot.localRotation = Quaternion.Lerp(startRot, targetRot, curved);
            }

            await UniTask.Yield();
        }

        slot.anchoredPosition = targetPos;
        slot.localScale = Vector3.one;
        if (useRotation)
        {
            slot.localRotation = targetRot;
        }
    }

    private async UniTask AnimateSlotCloseAsync(
        RectTransform slot,
        int delayMs,
        CancellationToken token)
    {
        await UniTask.Delay(delayMs, cancellationToken: token);

        Vector2 startPos = slot.anchoredPosition;
        Vector3 startScale = slot.localScale;
        Quaternion startRot = slot.localRotation;

        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            token.ThrowIfCancellationRequested();

            elapsed += Time.deltaTime;
            float t = easeCurve.Evaluate(elapsed / animationDuration);

            slot.anchoredPosition =
                Vector2.Lerp(startPos, closedPosition, t);
            slot.localScale = Vector3.Lerp(startScale, Vector3.zero, t);

            if (useRotation)
                slot.localRotation =
                    Quaternion.Lerp(startRot, Quaternion.identity, t);

            await UniTask.Yield();
        }

        slot.anchoredPosition = closedPosition;
        slot.localScale = Vector3.zero;
        slot.localRotation = Quaternion.identity;
    }

    private async UniTask AnimateScaleAsync(
        RectTransform target,
        Vector3 from,
        Vector3 to,
        float duration,
        CancellationToken token)
    {
        float elapsed = 0f;
        target.localScale = from;

        while (elapsed < duration)
        {
            token.ThrowIfCancellationRequested();

            elapsed += Time.deltaTime;
            target.localScale =
                Vector3.Lerp(from, to, elapsed / duration);

            await UniTask.Yield();
        }

        target.localScale = to;
    }

    private void OnDestroy()
    {
        animationCTS?.Cancel();
        animationCTS?.Dispose();
    }
}