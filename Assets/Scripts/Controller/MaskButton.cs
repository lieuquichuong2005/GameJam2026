using Cysharp.Threading.Tasks;
using EditorAttributes;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MaskButton : MonoBehaviour, IEntity
{
    [Required] [SerializeField] private Button _maskButton;
    [Required] [SerializeField] private GameObject _bloodLayer;
    [Required] [SerializeField] private RectTransform _eyePivot;

    [Header("Animation Settings")] [SerializeField]
    private Image _maskFillImage;

    [SerializeField] private float _fillDuration = 0.5f;
    [SerializeField] private float _eyeOpenDuration = 0.3f;
    [SerializeField] private AnimationCurve _fillCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private AnimationCurve _eyeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [SerializeField] private bool _isUsingFillAnimation;

    [Header("Eye Follow Mouse")] [SerializeField]
    private float _maxEyeOffset = 8f;

    [SerializeField] private float _eyeFollowSpeed = 12f;

    private bool _isPlayingAnimation;
    private bool _isShowingBloodMask;
    private Vector2 _eyeBaseLocalPos;

    public bool IsShowingBloodMask => _isShowingBloodMask;
    public static Action<bool> IsShowingBloodMaskAction;

    private void Awake()
    {
        _bloodLayer.SetActive(_isShowingBloodMask);
        _eyeBaseLocalPos = _eyePivot.localPosition;

        if (!_isUsingFillAnimation)
        {
            return;
        }

        if (_maskFillImage != null)
        {
            _maskFillImage.fillAmount = 0;
            _maskFillImage.gameObject.SetActive(false);
        }

        if (_eyePivot != null)
        {
            _eyePivot.localScale = Vector3.zero;
        }
    }

    public void OnUpdate(float deltaTime)
    {
        if (!_isShowingBloodMask) return;
        if (_eyePivot == null || _eyePivot == null) return;

        FollowMouse(deltaTime);
    }

    private void FollowMouse(float deltaTime)
    {
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _eyePivot,
                Input.mousePosition,
                null,
                out var localMousePos))
        {
            return;
        }

        Vector2 dir = localMousePos.normalized;
        Vector2 offset = dir * _maxEyeOffset;

        Vector2 finalPos = _eyeBaseLocalPos + offset;

        _eyePivot.localPosition = Vector2.Lerp(
            _eyePivot.localPosition,
            finalPos,
            deltaTime * _eyeFollowSpeed
        );
    }


    public void OnMaskButtonPressed()
    {
        if (_isPlayingAnimation)
        {
            return;
        }

        if (_isUsingFillAnimation)
        {
            _ = PlayAnimChangeMask();
        }
        else
        {
            _isShowingBloodMask = !_isShowingBloodMask;
            _bloodLayer.SetActive(_isShowingBloodMask);
        }

        IsShowingBloodMaskAction?.Invoke(_isShowingBloodMask);
    }

    private async UniTask PlayAnimChangeMask()
    {
        _isPlayingAnimation = true;

        try
        {
            if (_isShowingBloodMask)
            {
                _eyePivot.gameObject.SetActive(false);
                await AnimateEyeClose();
                await AnimateFillTransition(1f, 0f);

                _bloodLayer.SetActive(true);
                _isShowingBloodMask = false;
            }
            else
            {
                _bloodLayer.SetActive(false);
                await AnimateFillTransition(0f, 1f);
                await AnimateEyeOpen();

                _eyePivot.gameObject.SetActive(true);
                _isShowingBloodMask = true;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"[MaskButton] Animation error: {e.Message}");
        }
        finally
        {
            _isPlayingAnimation = false;
            _bloodLayer.SetActive(_isShowingBloodMask);
        }
    }

    private async UniTask AnimateFillTransition(float from, float to)
    {
        if (_maskFillImage == null) return;

        _maskFillImage.gameObject.SetActive(true);
        _maskFillImage.fillAmount = from;

        float elapsed = 0f;

        while (elapsed < _fillDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / _fillDuration);
            float curved = _fillCurve.Evaluate(t);

            _maskFillImage.fillAmount = Mathf.Lerp(from, to, curved);

            await UniTask.Yield();
        }

        _maskFillImage.fillAmount = to;

        if (to <= 0f)
        {
            _maskFillImage.gameObject.SetActive(false);
        }
    }

    private async UniTask AnimateEyeOpen()
    {
        if (_eyePivot == null) return;

        _eyePivot.gameObject.SetActive(true);
        _eyePivot.localScale = Vector3.zero;

        float elapsed = 0f;

        while (elapsed < _eyeOpenDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / _eyeOpenDuration);
            float curved = _eyeCurve.Evaluate(t);

            _eyePivot.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, curved);

            await UniTask.DelayFrame(3);
        }

        _eyePivot.localScale = Vector3.one;
    }

    private async UniTask AnimateEyeClose()
    {
        if (_eyePivot == null) return;

        _eyePivot.localScale = Vector3.one;

        float elapsed = 0f;

        while (elapsed < _eyeOpenDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / _eyeOpenDuration);
            float curved = _eyeCurve.Evaluate(t);

            _eyePivot.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, curved);

            await UniTask.DelayFrame(3);
        }

        _eyePivot.localScale = Vector3.zero;
        _eyePivot.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _isPlayingAnimation = false;
    }
}