using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour, IInventory
{
    [SerializeField] private TextMeshProUGUI _capacityText;
    [SerializeField] private Transform _inventoryStack;
    [Header("Properties")]
    [SerializeField] private int _capacity = 40;
    [SerializeField] private int _countColumns = 2;
    [SerializeField] private float _stackWidth = 0.5f;
    [SerializeField] private float _stackHeight = 1f;

    private readonly Stack<Transform> _items = new Stack<Transform>();

    public int CountItems => _items.Count;
    public bool IsFull => CountItems == _capacity;
    public bool IsEmpty => CountItems == 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IStackable item) && !IsFull)
        {
            AddItem(item);
            UpdateCapacityText();
        }
    }

    public Transform GetNextItem()
    {
        Transform item = _items.Pop();
        item.parent = null;
        UpdateCapacityText();

        return item;
    }

    private void AddItem(IStackable item)
    {
        Transform itemTransform = item.Stack();
        itemTransform.parent = _inventoryStack;
        itemTransform.localPosition = GetNextPositionInStack();
        _items.Push(itemTransform);
    }

    private void UpdateCapacityText()
    {
        _capacityText.text = $"{CountItems} / {_capacity}";
    }

    private Vector3 GetNextPositionInStack()
    {
        int columnCapacity = _capacity / _countColumns;

        int xIndex = (CountItems - 1) / columnCapacity;
        int yIndex = (CountItems - 1) % columnCapacity;

        float horizontalOffset = _stackWidth / _countColumns;
        float verticalOffset = _stackHeight / columnCapacity;

        float halfWidth = horizontalOffset / 2f;

        float x = xIndex * horizontalOffset - halfWidth;
        float y = yIndex * verticalOffset;

        var position = new Vector3(x, y, 0);
        return position;
    }
}