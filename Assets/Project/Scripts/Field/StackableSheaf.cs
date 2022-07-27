using UnityEngine;

public class StackableSheaf : MonoBehaviour, IStackable
{
    public void Stack(Transform parent, Vector3 position)
    {
        transform.parent = parent;
        transform.localPosition = position;
        GetComponent<Collider>().enabled = false;
    }
}