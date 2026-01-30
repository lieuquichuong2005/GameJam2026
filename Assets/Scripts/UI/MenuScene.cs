using UnityEngine;

public class MenuScene : InjectableMonoBehaviour
{
    [Inject] private IDataService data;
    [Inject] private IAudioService audio;

    protected override void Awake()
    {
        base.Awake();

        if (audio != null)
            audio.PlayBgm(AudioId.BgmMainMenu);
        else
            Debug.LogError($"Audio is null");
    }
}