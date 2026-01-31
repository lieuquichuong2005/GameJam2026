using UnityEngine;

public class Service : MonoBehaviour
{
    [Header("Audio")] [SerializeField] private AudioConfig audioConfig;
    [SerializeField] private AudioSourceProvider audioProvider;
    [SerializeField] private ItemMergeDatabase mergeDatabase;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        RegisterServices();
    }

    private void RegisterServices()
    {
        var audioService = new AudioService(audioConfig, audioProvider);
        Services.Register<IAudioService>(audioService);

        var dataService = new JsonDataService();
        dataService.Load();
        Services.Register<IDataService>(dataService);

        var inventoryService = new InventoryService();
        inventoryService.SetData(mergeDatabase);
        Services.Register(inventoryService);
    }
}