using UnityEngine;
using Cysharp.Threading.Tasks;

public class DoorInteracable : MonoBehaviour, IInteractable
{
    [Header("Interact")] [SerializeField] private float interactDistance = 1.2f;

    [SerializeField] private GameObject _safeLock;

    [Header("Slide Settings")] [SerializeField]
    private Vector3 slideOffset = new Vector3(-1.5f, 0, 0);

    [SerializeField] private float slideDuration = 0.5f;

    [Header("State")] [SerializeField] private bool openOnce = true;

    private bool isOpen = false;
    private bool isMoving = false;

    private Vector3 closedPos;
    private Vector3 openPos;

    public Transform Transform => transform;
    public float InteractDistance => interactDistance;
    public bool RequirePlayerNear { get; }

    private void Awake()
    {
        closedPos = transform.position;
        openPos = closedPos + slideOffset;
    }

    public bool CanInteract()
    {
        if (isMoving) return false;
        if (openOnce && isOpen) return false;
        return true;
    }

    public void Interact()
    {
        if (!CanInteract()) return;

        if (!isOpen)
            Open().Forget();

        else
            Close().Forget();
    }

    private async UniTaskVoid Open()
    {
        isMoving = true;
        await Slide(closedPos, openPos);
        isMoving = false;
        isOpen = true;
        _safeLock.SetActive(true);
    }

    private async UniTaskVoid Close()
    {
        if (openOnce) return;

        isMoving = true;
        await Slide(openPos, closedPos);
        isMoving = false;
        isOpen = false;
        _safeLock.SetActive(false);
    }

    private async UniTask Slide(Vector3 from, Vector3 to)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / slideDuration;
            transform.position = Vector3.Lerp(from, to, t);
            await UniTask.Yield();
        }

        transform.position = to;
    }
}