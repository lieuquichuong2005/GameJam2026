using UnityEngine;
using UnityEngine.EventSystems;

public class OpenBook : MonoBehaviour, IInteractable
{
    
    [Header("Interaction")]
    [SerializeField]
    private bool _requirePlayerNear = true;

    [SerializeField] private float _interactDistance = 0.5f;

    [Header("UI")][SerializeField] private GameObject BookUI;

    
    public Transform Transform => transform;
    public bool RequirePlayerNear => _requirePlayerNear;
    public float InteractDistance => _interactDistance;

    public bool CanInteract()
    {
        return true;
    }
    public void Interact()
    {
        BookUI.SetActive(true);
    }
}
