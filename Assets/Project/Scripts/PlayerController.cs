using UnityEngine;
using UnityEngine.InputSystem;

[AddComponentMenu("Player/Player Controller")]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private MovementController _movementController;
    [SerializeField] private Animator _animator;

    private Vector3 _rawInputMovement;
    private Vector3 _rotationDirection;

    public void OnMovePlayer(InputAction.CallbackContext context)
    {
        var inputDirection = context.ReadValue<Vector2>();
        _rawInputMovement = new Vector3(inputDirection.x, 0, inputDirection.y);
        _rotationDirection = new Vector3(inputDirection.x, 0, inputDirection.y);
    }

    private void Update()
    {
        UpdatePlayerMovement();
        UpdateAim();
    }

    private void UpdatePlayerMovement()
    {
        _movementController.SetDirection(_rawInputMovement);
    }

    private void UpdateAim()
    {
        _movementController.RotateAt(_rotationDirection);
    }
}