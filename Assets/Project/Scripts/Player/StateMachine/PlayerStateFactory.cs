using System.Collections.Generic;
using Project.Scripts.Player.StateMachine.States;
using UnityEngine;

namespace Project.Scripts.Player.StateMachine
{
    public enum PlayerState
    {
        None = -1,
        Idle,
        Walk,
        Attack
    }

    public class PlayerStateFactory
    {
        private readonly Dictionary<PlayerState, BasePlayerState> _states =
            new Dictionary<PlayerState, BasePlayerState>();

        public PlayerStateFactory(PlayerStateMachine stateMachine,
            PlayerMovementContext movementContext,
            PlayerAnimationContext animationContext)
        {
            _states.Add(PlayerState.Idle, new PlayerIdleState(stateMachine, movementContext, animationContext));
            _states.Add(PlayerState.Walk, new PlayerWalkState(stateMachine, movementContext, animationContext));
            _states.Add(PlayerState.Attack, new PlayerAttackState(stateMachine, movementContext, animationContext));
        }

        public BasePlayerState GetState(PlayerState state)
        {
            return _states.ContainsKey(state)
                ? _states[state]
                : null;
        }
    }
}