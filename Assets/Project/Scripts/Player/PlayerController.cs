using System;
using JetBrains.Annotations;
using Project.Scripts.Player;
using Project.Scripts.Player.StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[AddComponentMenu("Player/Player Controller")]
public class PlayerController : MonoBehaviour
{
    public event Action OnAttack;

    [SerializeField] private MovementController _movementController;
    [SerializeField] private Animator _animator;
    [SerializeField] private Image _joystick;

    private InputMaster _inputMaster;
    private PlayerStateMachine _playerStateMachine;
    private PlayerAnimationContext _playerAnimationContext;
    private PlayerMovementContext _playerMovementContext;

    private void Awake()
    {
        _inputMaster = new InputMaster();
        _playerMovementContext = new PlayerMovementContext(_movementController);
        _playerAnimationContext = new PlayerAnimationContext(_animator);

        _playerStateMachine = new PlayerStateMachine(_playerMovementContext, _playerAnimationContext);
        _playerStateMachine.SwitchState(PlayerState.Idle);
    }

    private void OnEnable()
    {
        _inputMaster.Enable();

        _inputMaster.Player.Move.performed += OnMovePlayer;
        _inputMaster.Player.Move.canceled += OnMovePlayer;

        _inputMaster.Player.Attack.started += OnAttackPlayer;
        _inputMaster.Player.Attack.performed += OnAttackPlayer;
        _inputMaster.Player.Attack.canceled += OnAttackPlayerCanceled;

        _inputMaster.Player.TouchPress.started += OnTouchPressed;
        _inputMaster.Player.TouchPress.canceled += OnTouchCanceled;
    }

    private void OnDisable()
    {
        _inputMaster.Disable();

        _inputMaster.Player.Move.performed -= OnMovePlayer;
        _inputMaster.Player.Move.canceled -= OnMovePlayer;

        _inputMaster.Player.Attack.started -= OnAttackPlayer;
        _inputMaster.Player.Attack.performed -= OnAttackPlayer;
        _inputMaster.Player.Attack.canceled -= OnAttackPlayerCanceled;

        _inputMaster.Player.TouchPress.started -= OnTouchPressed;
        _inputMaster.Player.TouchPress.canceled -= OnTouchCanceled;
    }

    private void Update()
    {
        _playerStateMachine.Update();
        _playerMovementContext.Update();
    }

    [UsedImplicitly]
    public void OnAttackEnded()
    {
        OnAttack?.Invoke();
    }

    private void OnMovePlayer(InputAction.CallbackContext context)
    {
        var inputDirection = context.ReadValue<Vector2>();
        _playerMovementContext.RawInputMovement = new Vector3(inputDirection.x, 0, inputDirection.y);
        _playerMovementContext.RotationDirection = new Vector3(inputDirection.x, 0, inputDirection.y);
    }

    private void OnAttackPlayer(InputAction.CallbackContext context)
    {
        _playerMovementContext.IsAttack = true;
    }

    private void OnAttackPlayerCanceled(InputAction.CallbackContext context)
    {
        _playerMovementContext.IsAttack = false;
    }

    private void OnTouchPressed(InputAction.CallbackContext context)
    {
        var touchPosition = _inputMaster.Player.TouchPosition.ReadValue<Vector2>();

        if (touchPosition.x < Screen.width / 2f)
        {
            _joystick.transform.position = touchPosition;
            _joystick.enabled = true;
        }
    }

    private void OnTouchCanceled(InputAction.CallbackContext context)
    {
        _joystick.enabled = false;
    }
}