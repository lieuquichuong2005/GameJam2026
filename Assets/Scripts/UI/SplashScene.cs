using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScene : MonoBehaviour
{
    [Header("Audio")] [SerializeField] private AudioConfig audioConfig;
    [SerializeField] private AudioSourceProvider audioProvider;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        RegisterServices();
        LoadFirstScene();
    }

    private void RegisterServices()
    {
        var audioService = new AudioService(audioConfig, audioProvider);
        Services.Register<IAudioService>(audioService);

        var dataService = new JsonDataService();
        dataService.Load();
        Services.Register<IDataService>(dataService);
    }


    private void LoadFirstScene()
    {
        SceneManager.LoadScene(1);
    }
}