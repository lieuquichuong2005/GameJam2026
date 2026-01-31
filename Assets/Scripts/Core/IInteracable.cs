using UnityEngine;

public interface IInteractable
{
    Transform Transform { get; }
    float InteractDistance { get; }

    bool RequirePlayerNear { get; }   
    bool CanInteract();
    void Interact();
}