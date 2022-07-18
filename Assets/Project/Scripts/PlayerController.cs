using UnityEngine;
using UnityEngine.InputSystem;

[AddComponentMenu("Player/Player Controller")]
public class PlayerController : MonoBehaviour
{
    [Header("Sub Behaviours")]
    [SerializeField] private MovementController _movementController;

    [Header("Input Settings")]
    [SerializeField] private PlayerInput _playerInput;

    private Vector3 _rawInputMovement;
    private Vector3 _rotationDirection;

    private string _currentControlScheme;
    private const string GAMEPAD_CONTROL_SCHEME = "Gamepad";
    private const string KEYBOARD_CONTROL_SCHEME = "Keyboard And Mouse";
    private const string ACTION_MAP_PLAYER_CONTROLS = "Player";
    private const string ACTION_MAP_MENU_CONTROLS = "UI";

    public void OnControlsChanged()
    {
        Debug.LogWarning("With reloading scene, this method is no longer called.\nThis issue: https://forum.unity.com/threads/controls-changed-event-not-invoking-on-scene-change.885592/");
        if (_playerInput.currentControlScheme != _currentControlScheme)
        {
            _currentControlScheme = _playerInput.currentControlScheme;
        }
    }

    public void OnMovePlayer(InputAction.CallbackContext context)
    {
        var inputDirection = context.ReadValue<Vector2>();
        _rawInputMovement = new Vector3(inputDirection.x, 0, inputDirection.y);
    }

    public void OnAnalogAimPlayer(InputAction.CallbackContext context)
    {
        var inputRotation = context.ReadValue<Vector2>();
        _rotationDirection = new Vector3(inputRotation.x, 0, inputRotation.y);
    }

    public void SwitchFocusedPlayerControlScheme(bool isPaused)
    {
        switch (isPaused)
        {
            case true:
                EnablePauseMenuControls();
                break;

            case false:
                EnableGameplayControls();
                break;
        }
    }

    public void EnablePauseMenuControls()
    {
        _playerInput.SwitchCurrentActionMap(ACTION_MAP_MENU_CONTROLS);
    }

    public void EnableGameplayControls()
    {
        _playerInput.SwitchCurrentActionMap(ACTION_MAP_PLAYER_CONTROLS);
    }

    private void Awake()
    {
        _currentControlScheme = _playerInput.currentControlScheme;
    }

    private void Update()
    {
        UpdateControlScheme();
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

    private void UpdateControlScheme()
    {
        _currentControlScheme = _playerInput.currentControlScheme;
    }
}