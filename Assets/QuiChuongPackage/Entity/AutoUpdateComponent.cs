using UnityEngine;

public class AutoUpdateComponent : MonoBehaviour
{
    private IEntity _entity;
    private LevelScene _level;

    private void Awake()
    {
        _entity = GetComponent<IEntity>();
        _level = FindFirstObjectByType<LevelScene>();

        if (_entity == null)
        {
            Debug.LogError($"{name} has AutoUpdate but no IEntity");
            enabled = false;
            return;
        }
    }

    private void OnEnable()
    {
        _level.Register(_entity);
    }

    private void OnDisable()
    {
        _level.Unregister(_entity);
    }
}