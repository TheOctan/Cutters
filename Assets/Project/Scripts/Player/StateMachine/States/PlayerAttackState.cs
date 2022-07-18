using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Project.Scripts.Player.StateMachine.States
{
    public class PlayerAttackState : BasePlayerState
    {
        public PlayerAttackState(PlayerStateMachine stateMachine,
            PlayerMovementContext movementContext,
            PlayerAnimationContext animationContext)
            : base(stateMachine, movementContext, animationContext)
        {
        }

        public override void EnterState()
        {
            AnimationContext.IsAttack = true;
        }

        public override void UpdateState()
        {
            if (AnimationContext.IsAnimationAttack && AnimationContext.IsAttack)
            {
                AnimationContext.IsAttack = false;
                ExitFromStateByDelayAsync(AnimationContext.CurrentAnimationLenght - 0.3f);
            }
        }

        public override void ExitState()
        {
        }

        private async void ExitFromStateByDelayAsync(float delay)
        {
            await Task.Delay((int)(delay * 1000));

            if (MovementContext.IsMoved)
            {
                
                SwitchState(PlayerState.Walk);
            }
            else
            {
                SwitchState(PlayerState.Idle);
            }
        }
    }
}