namespace Project.Scripts.Player.StateMachine.States
{
    public abstract class BasePlayerState
    {
        protected readonly PlayerMovementContext MovementContext;
        protected readonly PlayerAnimationContext AnimationContext;

        private readonly PlayerStateMachine _stateMachine;

        protected BasePlayerState(PlayerStateMachine stateMachine,
            PlayerMovementContext movementContext, 
            PlayerAnimationContext animationContext)
        {
            _stateMachine = stateMachine;
            AnimationContext = animationContext;
            MovementContext = movementContext;
        }

        public abstract void EnterState();
        public abstract void UpdateState();
        public abstract void ExitState();
        protected void SwitchState(PlayerState newState)
        {
            _stateMachine.SwitchState(newState);
        }
    }
}