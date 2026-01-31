using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SafeCodeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI displayText;

    private string currentInput = "";

    public event Action<string> OnSubmit;
    public event Action OnCancel;

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