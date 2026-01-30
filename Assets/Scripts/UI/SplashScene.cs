using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using System.Threading;

public class SplashScene : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _loadingText;
    [SerializeField] private int nextSceneIndex = 1;

    private CancellationTokenSource _cts;

    private void Awake()
    {
        _cts = new CancellationTokenSource();
        RunSplashAsync(_cts.Token).Forget();
    }

    private async UniTaskVoid RunSplashAsync(CancellationToken token)
    {
        var loadingTask = LoadingTextEffect(token);

        await UniTask.Delay(5000, cancellationToken: token);

        _cts.Cancel();

        SceneManager.LoadScene(nextSceneIndex);
    }

    private async UniTask LoadingTextEffect(CancellationToken token)
    {
        string baseText = "Loading";
        int dotCount = 0;

        while (!token.IsCancellationRequested)
        {
            dotCount = (dotCount + 1) % 7;
            _loadingText.text = baseText + new string('.', dotCount);

            await UniTask.Delay(400, cancellationToken: token);
        }
    }

    private void OnDestroy()
    {
        _cts?.Cancel();
        _cts?.Dispose();
    }
}