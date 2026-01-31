using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using EditorAttributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Quản Lí UI. Không Quản Lí CoreGameplay
/// </summary>
public class LevelScene : InjectableMonoBehaviour
{
    [Inject] private InventoryService inventory;

    [Required] [SerializeField] private Canvas _canvas;
    [Required] [SerializeField] private LevelView _levelView;
    [Required] [SerializeField] private Camera _mainCamera;
    [Required] [SerializeField] private FlyingItemEffect _flyingItemPrefab;
    [Required] [SerializeField] private RectTransform _inventoryRoot;
    [Required] [SerializeField] private InventoryAnimatorUniTask _inventoryPanel;

    [SerializeField] private Room _firstRoomType;
    [SerializeField] private ItemMergeDatabase _itemMergeDatabase;

    [SerializeField] private List<InventoryItemView> inventorySlots = new List<InventoryItemView>();
    private readonly List<IEntity> _entities = new();
    private bool _paused;
    private float _lastTime;
    private bool _isPlaying;

    public void InitInventorySlot(InventoryService service)
    {
        inventory = service;
    }

    private void AddItemToSlot(InventoryItem item)
    {
        var emptySlot = inventorySlots.Find(slot => slot.IsEmpty());

        if (emptySlot == null)
        {
            return;
        }

        emptySlot.SetItem(item);
    }

    private void RemoveItemFromSlot(InventoryItem item)
    {
        var itemSlot = inventorySlots.Find(slot => slot.Data == item);
        itemSlot.ClearSlot();
    }

    protected override void Awake()
    {
        base.Awake();
        if (inventory == null)
        {
            inventory = new InventoryService();
            Services.Register(inventory);
        }

        inventory.SetData(_itemMergeDatabase);
        _lastTime = Time.realtimeSinceStartup;
        InitInventorySlot(inventory);

        foreach (var slot in inventorySlots)
        {
            slot.Init(inventory);
        }

        _isPlaying = true;

        _levelView.EnterRoom(_firstRoomType);
    }

    private void OnEnable()
    {
        PlayAreaInput.OnClick += OnClicked;
        inventory.OnInventoryChanged += Rebuild;
        inventory.OnItemAdded += AddItemToSlot;
        inventory.OnItemRemoved += RemoveItemFromSlot;
    }

    private void Rebuild()
    {
        ClearAllSlots();

        foreach (var item in inventory.Items)
            AssignItemToSlot(item);
    }

    private void ClearAllSlots()
    {
        foreach (var slot in inventorySlots)
            slot.ClearSlot();
    }

    private void AssignItemToSlot(InventoryItem item)
    {
        var slot = inventorySlots.Find(s => s.IsEmpty());
        if (slot != null)
            slot.SetItem(item);
    }

    private void OnDisable()
    {
        PlayAreaInput.OnClick -= OnClicked;
        inventory.OnInventoryChanged -= Rebuild;
        inventory.OnItemAdded -= AddItemToSlot;
        inventory.OnItemRemoved -= RemoveItemFromSlot;
    }

    private void Update()
    {
        HandleUpdate();
        // HandleInput();
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
        }
    }

    public void Unregister(IEntity entity)
    {
        _entities.Remove(entity);
    }

    public void Pause()
    {
        _paused = true;
    }

    public void Resume()
    {
        _paused = false;
        _lastTime = Time.realtimeSinceStartup;
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

    private void OnClicked(Vector2 scenepoint)
    {
        if (!_isPlaying)
        {
            return;
        }

        var position = _mainCamera.ScreenToWorldPoint(scenepoint);
        _ = _levelView.PressOnPosition(position);
    }

    public async UniTask PlayPickupItemEffect(
        InventoryItem item,
        Vector3 worldItemPos)
    {
        var slot = inventorySlots.Find(s => s.IsEmpty());
        if (slot == null) return;

        Vector2 startScreen =
            _mainCamera.WorldToScreenPoint(worldItemPos);

        Vector2 endScreen = _inventoryPanel.transform.position + new Vector3(25, 0, 0);

        var flying = Instantiate(_flyingItemPrefab, _canvas.transform);
        await flying.Fly(item.icon, startScreen, endScreen);

        Destroy(flying.gameObject);

        _inventoryPanel.OpenInventory();

        inventory.AddItem(item);
    }
}