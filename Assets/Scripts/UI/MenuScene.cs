public class MenuScene : InjectableMonoBehaviour
{
    [Inject] private IDataService data;
    [Inject] private IAudioService audio;

    private void Awake()
    {
        // audio.PlayBgm(AudioId.BgmMainMenu);
    }
    
    
    
}