using Assets.Scripts;
using UnityEngine;

[AddComponentMenu("Player/Movement Controller")]
public class MovementController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbodyComponent;

    [Header("Movement")]
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private float _acceleration = 12f;

    [Header("Rotation")] 
    [SerializeField] private float _turnSpeed = 12f;
    [SerializeField] private bool _velocityDependent = true;
    [SerializeField] private bool _rotateWithMovement = true;
    [SerializeField] private RotationType _rotationType;

    [Space] 
    [SerializeField] private bool _alignToCamera = true;

    private Transform _cameraTransform;

    private Vector3 _rawMovementDirection;
    private Vector3 _movementDirection;
    private Vector3 _rotationDirection;
    private Vector3 _velocity;

    public float CurrentSpeed => _velocity.magnitude;

    public void SetDirection(Vector3 direction)
    {
        _rawMovementDirection = _alignToCamera 
            ? AlignToCamera(direction) 
            : direction;
    }

    public void RotateAt(Vector3 direction)
    {
        _rotationDirection = _alignToCamera 
            ? AlignToCamera(direction) 
            : direction;
    }

    public void LookAt(Vector3 point)
    {
        Vector3 position = transform.position;
        var heightCorrectedPoint = new Vector3(point.x, position.y, point.z);
        _rotationDirection = (heightCorrectedPoint - position).normalized;
    }

    private void Start()
    {
        if (!ReferenceEquals(Camera.main, null))
        {
            _cameraTransform = Camera.main.transform;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
        RotatePlayer();
        DrawDebugLines();
    }

    private void MovePlayer()
    {
        float tangentSpeed = _acceleration * Time.fixedDeltaTime;
        _movementDirection = AccelerateDirection(_movementDirection, _rawMovementDirection, tangentSpeed);
        _velocity = _movementDirection * _movementSpeed;
        Vector3 movement = _velocity * Time.fixedDeltaTime;

        _rigidbodyComponent.MovePosition(_rigidbodyComponent.position + movement);
    }

    private void RotatePlayer()
    {
        if (_rotateWithMovement && _rotationDirection == Vector3.zero)
        {
            RotateTowards(
                _rotationType == RotationType.MotionDependment 
                ? _movementDirection 
                : _rawMovementDirection);
        }
        else
        {
            RotateTowards(_rotationDirection);
        }
    }

    private void RotateTowards(Vector3 direction)
    {
        if (direction.sqrMagnitude > 0.0025f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            float turnStep = _turnSpeed * Time.fixedDeltaTime;
            if (_velocityDependent)
            {
                turnStep *= direction.magnitude;
            }

            Quaternion rotation = AccelerateRotation(_rigidbodyComponent.rotation, targetRotation, turnStep);

            _rigidbodyComponent.MoveRotation(rotation);
        }
    }

    private static Vector3 AccelerateDirection(Vector3 direction, Vector3 targetDirection, float tangentSpeed)
    {
        return tangentSpeed > 0 
            ? Vector3.Lerp(direction, targetDirection, tangentSpeed) 
            : targetDirection;
    }

    private static Quaternion AccelerateRotation(Quaternion rotation, Quaternion targetRotation, float turnSpeed)
    {
        return turnSpeed > 0 
            ? Quaternion.Slerp(rotation, targetRotation, turnSpeed) 
            : targetRotation;
    }

    private Vector3 AlignToCamera(Vector3 direction)
    {
        Vector3 cameraForward = _cameraTransform.forward;
        cameraForward.y = 0;

        return Quaternion.LookRotation(cameraForward) * direction;
    }

    private void DrawDebugLines()
    {
        Debug.DrawRay(_rigidbodyComponent.position + Vector3.up, _movementDirection * 1.5f, Color.red);
        Debug.DrawRay(_rigidbodyComponent.position + Vector3.up, _rawMovementDirection * 1.5f, Color.green);
        Debug.DrawRay(_rigidbodyComponent.position + Vector3.up, _rotationDirection * 1.5f, Color.yellow);
        Debug.DrawRay(_rigidbodyComponent.position + Vector3.up, transform.forward * 1.5f, Color.blue);
    }
}