using UnityEngine;

public interface IInventory
{
    bool IsFull { get; }
    bool IsEmpty { get; }
    int CountItems { get; }
    Transform GetNextItem();
}