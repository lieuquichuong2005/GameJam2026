using Cysharp.Threading.Tasks;
using EditorAttributes;
using UnityEngine;
using UnityEngine.UI;

public class FlyingItemEffect : MonoBehaviour
{
    [Required] [SerializeField] private Image _icon;
    [SerializeField] private float _flyingDuration;

    public async UniTask Fly(Sprite item,
        Vector2 startScreenPos,
        Vector2 endScreenPos)
    {
        _icon.sprite = item;
        RectTransform rt = (RectTransform)transform;
        rt.position = startScreenPos;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / _flyingDuration;
            rt.position = Vector2.Lerp(startScreenPos, endScreenPos, t);
            await UniTask.Yield();
        }

        rt.position = endScreenPos;
    }
}