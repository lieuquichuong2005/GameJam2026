using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SafeCodeUI : InjectableMonoBehaviour
{
    [Inject] private AudioService audioService;

    [SerializeField] private TextMeshProUGUI displayText;

    private string currentInput = "";

    public event Action<string> OnSubmit;
    public event Action OnCancel;

    protected override void Awake()
    {
        base.Awake();
    }

    public void Show()
    {
        currentInput = "";
        UpdateDisplay();
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void PressNumber(string num)
    {
        if (currentInput.Length >= 4) return;
        audioService.Play(AudioId.SafeLockButtonPress);
        currentInput += num;
        UpdateDisplay();
    }

    public void PressSubmit()
    {
        OnSubmit?.Invoke(currentInput);
    }

    public void PressCancel()
    {
        OnCancel?.Invoke();
    }

    public void PlayError()
    {
        Debug.Log("[SAFE UI] Wrong code!");
        currentInput = "";
        UpdateDisplay();
        // TODO: shake / sound
    }

    private void UpdateDisplay()
    {
        displayText.text = currentInput;
    }
}