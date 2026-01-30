using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Quán Lí UI. Không Quản Lí CoreGaneplay
/// </summary>
public class LevelScene : MonoBehaviour
{
    [SerializeField] private LevelView levelView;
    [SerializeField] private Camera mainCamera;

    private readonly List<IEntity> _entities = new();
    private bool _paused;

    private float _lastTime;

    private void Awake()
    {
        _lastTime = Time.realtimeSinceStartup;
    }

    private void Update()
    {
        HandleUpdate();
        HandleInput();
    }

    private void HandleUpdate()
    {
        if (_paused) return;

        float now = Time.realtimeSinceStartup;
        float deltaTime = now - _lastTime;
        _lastTime = now;

        for (int i = 0; i < _entities.Count; i++)
        {
            _entities[i].OnUpdate(deltaTime);
        }
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            levelView.OnClickWorld(worldPos);
        }
    }


    public void Register(IEntity entity)
    {
        if (!_entities.Contains(entity))
        {
            _entities.Add(entity);
            Debug.Log($"[LEVEL] Register entity: {entity}");
        }
    }

    public void Unregister(IEntity entity)
    {
        _entities.Remove(entity);
        Debug.Log($"[LEVEL] Unregister entity: {entity}");
    }

    public void Pause()
    {
        _paused = true;
        Debug.Log("[LEVEL] Paused");
    }

    public void Resume()
    {
        _paused = false;
        _lastTime = Time.realtimeSinceStartup;
        Debug.Log("[LEVEL] Resumed");
    }
}