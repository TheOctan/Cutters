namespace Project.Scripts.Player.StateMachine.States
{
    public class PlayerWalkState : BasePlayerState
    {
        public PlayerWalkState(PlayerStateMachine stateMachine,
            PlayerMovementContext movementContext, 
            PlayerAnimationContext animationContext)
            : base(stateMachine, movementContext, animationContext)
        {
        }
        
        public override void EnterState()
        {
            AnimationContext.IsWalk = true;
            MovementContext.Start();
        }

        public override void UpdateState()
        {
            AnimationContext.WalkingSpeed = MovementContext.CurrentSpeed;

            if (MovementContext.IsAttack)
            {
                SwitchState(PlayerState.Attack);
            }
            else if (!MovementContext.IsMoved)
            {
                SwitchState(PlayerState.Idle);
            }
        }

        public override void ExitState()
        {
            AnimationContext.IsWalk = false;
            MovementContext.Stop();
        }
    }
}