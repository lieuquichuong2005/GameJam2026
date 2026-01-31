using UnityEngine;
using UnityEngine.UI;

public class NorteInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _noteLayer;
    [SerializeField] private Button _closeButton;

    public Transform Transform => transform;
    public float InteractDistance => 0.5f;
    public bool RequirePlayerNear { get; }

    public bool CanInteract()
    {
        return true;
    }

    public void Interact()
    {
        _noteLayer.SetActive(true);
    }

    public void OnCloseButtonPressed()
    {
        _noteLayer.SetActive(false);
    }
}