using UnityEngine;

[RequireComponent(typeof(DoorController))]
public class DoorToggle : MonoBehaviour
{
    private DoorController _doorController;

    private void Awake()
    {
        _doorController = GetComponent<DoorController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController _))
        {
            _doorController.ToggleDoor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerController _))
        {
            _doorController.ToggleDoor();
        }
    }
}