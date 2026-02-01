using System;
using UnityEngine;

public class SafeController : InjectableMonoBehaviour
{
    [Inject] private AudioService _audioService;

    [SerializeField] private string correctCode = "1234";
    [SerializeField] private GameObject rewardItem;
    [SerializeField] private SafeCodeUI codeUI;

    public Action<bool> IsUnlocked;

    public bool IsOpened { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        codeUI.Hide();
        codeUI.OnSubmit += CheckCode;
        codeUI.OnCancel += OnCancel;
    }

    public void OpenInput()
    {
        if (IsOpened) return;

        codeUI.Show();
    }

    private void CheckCode(string input)
    {
        if (input == correctCode)
        {
            OpenSafe();
            _audioService.Play(AudioId.CorrectSafeLockPassword);
            IsUnlocked?.Invoke(true);
        }
        else
        {
            codeUI.PlayError();
            _audioService.Play(AudioId.WrongSafeLockPassword);
            IsUnlocked?.Invoke(false);
        }
    }

    private void OpenSafe()
    {
        IsOpened = true;

        codeUI.Hide();

        if (rewardItem != null)
            rewardItem.SetActive(true);

        Debug.Log("[SAFE] Opened!");
    }

    private void OnCancel()
    {
        codeUI.Hide();
    }
}