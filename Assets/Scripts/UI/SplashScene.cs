using UnityEngine;

public class SplashScene : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioConfig audioConfig;
    [SerializeField] private AudioSourceProvider audioProvider;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        var audioService = new AudioService(audioConfig, audioProvider);
        Services.Register<IAudioService>(audioService);
    }
}