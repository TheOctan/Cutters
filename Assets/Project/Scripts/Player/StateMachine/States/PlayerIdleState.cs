namespace Project.Scripts.Player.StateMachine.States
{
    public class PlayerIdleState : BasePlayerState
    {
        public PlayerIdleState(PlayerStateMachine stateMachine,
            PlayerMovementContext movementContext, 
            PlayerAnimationContext animationContext)
            : base(stateMachine, movementContext, animationContext)
        {
        }

        public override void EnterState()
        {
        }

        public override void UpdateState()
        {
            if (MovementContext.IsAttack)
            {
                SwitchState(PlayerState.Attack);
            }
            else if (MovementContext.IsMoved)
            {
                SwitchState(PlayerState.Walk);
            }
        }

        public override void ExitState()
        {
        }
    }
}