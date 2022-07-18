using UnityEngine;

namespace Project.Scripts.Player
{
    public class PlayerAnimationContext
    {
        private const string IS_WALK_ANIMATION_KEY = "IsWalk";
        private const string IS_ATTACK_ANIMATION_KEY = "IsAttack";
        private const string WALKING_SPEED = "WalkingSpeed";
        private const string ATTACK_ANIMATION_NAME = "DownwardAttack";

        private readonly Animator _animator;

        private readonly int _isWalkHash;
        private readonly int _isAttackHash;
        private readonly int _walkingSpeedHash;

        public bool IsAnimationAttack => 
            _animator.GetCurrentAnimatorStateInfo(0).IsName(ATTACK_ANIMATION_NAME);
        public float CurrentAnimationLenght => _animator.GetCurrentAnimatorStateInfo(0).length; 

        public bool IsWalk
        {
            get => _animator.GetBool(_isWalkHash);
            set => _animator.SetBool(_isWalkHash, value);
        }

        public bool IsAttack
        {
            get => _animator.GetBool(_isAttackHash);
            set => _animator.SetBool(_isAttackHash, value);
        }

        public float WalkingSpeed
        {
            get => _animator.GetFloat(_walkingSpeedHash);
            set => _animator.SetFloat(_walkingSpeedHash, value);
        }

        public PlayerAnimationContext(Animator animator)
        {
            _animator = animator;

            _isWalkHash = Animator.StringToHash(IS_WALK_ANIMATION_KEY);
            _isAttackHash = Animator.StringToHash(IS_ATTACK_ANIMATION_KEY);
            _walkingSpeedHash = Animator.StringToHash(WALKING_SPEED);
        }
    }
}