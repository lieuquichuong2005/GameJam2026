using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Quản Lí UI. Không Quản Lí CoreGameplay
/// </summary>
public class LevelScene : InjectableMonoBehaviour
{
    [Inject] private InventoryService inventory;

    [SerializeField] private LevelView _levelView;
    [SerializeField] private Camera _mainCamera;

    [SerializeField] private List<InventoryItemView> inventorySlots = new List<InventoryItemView>();
    private readonly List<IEntity> _entities = new();
    private bool _paused;
    private float _lastTime;
    private bool _isPlaying;

    public void InitInventorySlot(InventoryService service)
    {
        inventory = service;
        inventory.OnItemAdded += AddItemToSlot;
    }

    private void AddItemToSlot(InventoryItem item)
    {
        var emptySlot = inventorySlots.Find(slot => slot.IsEmpty());

        if (emptySlot == null)
        {
            Debug.LogWarning("Inventory full - no empty slots");
            return;
        }

        emptySlot.SetItem(item);
        Debug.Log($"Added {item.itemId} to inventory slot");
    }

    protected override void Awake()
    {
        // base.Awake();
        inventory = new InventoryService();
        Services.Register(inventory);
        _lastTime = Time.realtimeSinceStartup;
        InitInventorySlot(inventory);

        _isPlaying = true;
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
            Vector2 worldPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            _levelView.PressOnPosition(worldPos);
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

    public void OnPlayAreaPressed(BaseEventData eventData)
    {
        // if (!_isPlaying)
        // {
        //     Debug.Log("[LEVEL] OnPlayAreaPressed");
        //     return;
        // }
        //
        // var data = (PointerEventData)eventData;
        // var position = _mainCamera.ScreenToWorldPoint(data.position);
        // _ = _levelView.PressOnPosition(position);
        // Debug.Log($"Pressed position: {position}");
    }
}