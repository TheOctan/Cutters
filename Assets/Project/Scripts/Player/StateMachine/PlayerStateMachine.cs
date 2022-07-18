using Project.Scripts.Player.StateMachine.States;
using UnityEngine;

namespace Project.Scripts.Player.StateMachine
{
    public class PlayerStateMachine
    {
        private readonly PlayerStateFactory _states;
        private BasePlayerState _currentState;
        private PlayerState _currentStateType = PlayerState.None;

        public PlayerStateMachine(PlayerMovementContext movementContext,
            PlayerAnimationContext animationContext)
        {
            _states = new PlayerStateFactory(this, movementContext, animationContext);
        }

        public void SwitchState(PlayerState state)
        {
            if (_currentStateType == state)
            {
                return;
            }
            
            _currentState?.ExitState();
            BasePlayerState newState = _states.GetState(state);
            newState.EnterState();

            _currentState = newState;
            _currentStateType = state;

            Debug.Log(_currentStateType);
        }

        public void Update()
        {
            _currentState?.UpdateState();
        }
    }
}