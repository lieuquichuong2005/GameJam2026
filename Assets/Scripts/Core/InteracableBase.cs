using UnityEngine;

public abstract class InteractableBase : MonoBehaviour, IInteractable
{
    [SerializeField] protected float interactDistance = 1.5f;

    public Transform Transform => transform;
    public float InteractDistance => interactDistance;

    public virtual bool CanInteract() => true;

    public abstract void Interact();

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactDistance);
    }
#endif
}