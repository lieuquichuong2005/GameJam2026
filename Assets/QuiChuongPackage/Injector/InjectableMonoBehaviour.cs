using UnityEngine;

public abstract class InjectableMonoBehaviour : MonoBehaviour
{
    protected virtual void Awake()
    {
        Injector.Inject(this);
    }
}