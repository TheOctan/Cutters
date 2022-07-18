using UnityEngine;

namespace Project.Scripts.Player
{
    public class PlayerMovementContext
    {
        private readonly MovementController _movementController;
        private bool _enable = true;

        public Vector3 RawInputMovement { get; set; }
        public Vector3 RotationDirection { get; set; }

        public bool IsAttack { get; set; }
        public bool IsMoved => RawInputMovement.x != 0 || RawInputMovement.z != 0;
        public float CurrentSpeed => _movementController.CurrentSpeed;

        public PlayerMovementContext(MovementController movementController)
        {
            _movementController = movementController;
        }

        public void Update()
        {
            if (!_enable)
            {
                return;
            }

            _movementController.SetDirection(RawInputMovement);
            _movementController.RotateAt(RotationDirection);
        }

        public void Start()
        {
            _enable = true;
        }

        public void Stop()
        {
            _enable = false;
            _movementController.SetDirection(Vector3.zero);
        }
    }
}