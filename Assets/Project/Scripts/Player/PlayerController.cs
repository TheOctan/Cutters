using System;
using JetBrains.Annotations;
using Project.Scripts.Player;
using Project.Scripts.Player.StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

[AddComponentMenu("Player/Player Controller")]
public class PlayerController : MonoBehaviour
{
    public event Action OnAttack;

    [SerializeField] private MovementController _movementController;
    [SerializeField] private Animator _animator;

    private PlayerStateMachine _playerStateMachine;
    private PlayerAnimationContext _playerAnimationContext;
    private PlayerMovementContext _playerMovementContext;

    private void Awake()
    {
        _playerMovementContext = new PlayerMovementContext(_movementController);
        _playerAnimationContext = new PlayerAnimationContext(_animator);

        _playerStateMachine = new PlayerStateMachine(_playerMovementContext, _playerAnimationContext);
        _playerStateMachine.SwitchState(PlayerState.Idle);
    }

    [UsedImplicitly]
    public void OnMovePlayer(InputAction.CallbackContext context)
    {
        var inputDirection = context.ReadValue<Vector2>();
        _playerMovementContext.RawInputMovement = new Vector3(inputDirection.x, 0, inputDirection.y);
        _playerMovementContext.RotationDirection = new Vector3(inputDirection.x, 0, inputDirection.y);
    }

    [UsedImplicitly]
    public void OnAttackPlayer(InputAction.CallbackContext context)
    {
        if (context.started || context.performed)
        {
            _playerMovementContext.IsAttack = true;
        }
        else if(context.canceled)
        {
            _playerMovementContext.IsAttack = false;
        }
    }

    [UsedImplicitly]
    public void OnAttackEnded()
    {
        OnAttack?.Invoke();
    }

    private void Update()
    {
        _playerStateMachine.Update();
        _playerMovementContext.Update();
    }
}