using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _capacityText;
    [SerializeField] private Transform _inventoryStack;
    [Header("Properties")]
    [SerializeField] private int _capacity = 40;
    [SerializeField] private int _countColumns = 2;
    [SerializeField] private float _stackWidth = 0.5f;
    [SerializeField] private float _stackHeight = 1f;

    private int _countItems;

    private bool IsFull => _countItems == _capacity;

    private void UpdateCapacityText()
    {
        _capacityText.text = $"{_countItems} / {_capacity}";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IStackable item))
        {
            if (!IsFull)
            {
                _countItems++;
                item.Stack(_inventoryStack, GetNextPositionInStack());
                UpdateCapacityText();
            }
        }
    }

    private Vector3 GetNextPositionInStack()
    {
        int columnCapacity = _capacity / _countColumns;

        int xIndex = (_countItems - 1) / columnCapacity;
        int yIndex = (_countItems - 1) % columnCapacity;

        float horizontalOffset = _stackWidth / _countColumns;
        float verticalOffset = _stackHeight / columnCapacity;

        float halfWidth = horizontalOffset / 2f;

        float x = xIndex * horizontalOffset - halfWidth;
        float y = yIndex * verticalOffset;

        var position = new Vector3(x, y, 0);
        return position;
    }
}