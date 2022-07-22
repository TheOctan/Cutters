using UnityEngine;

[RequireComponent(typeof(Animation))]
public class DoorController : MonoBehaviour
{
    private enum DoorState
    {
        Open,
        Closed
    }

    [SerializeField] private DoorState _initialState = DoorState.Closed;
    [SerializeField] private Collider _doorCollider;

    [Header("Animation")]
    [SerializeField] private float _animationSpeed = 1;
    [SerializeField] private AnimationClip _openAnimation;
    [SerializeField] private AnimationClip _closeAnimation;

    private Animation _animator;
    private DoorState _currentState;

    private DoorState CurrentState {
        get => _currentState;
        set
        {
            _currentState = value;
            Animate();
        }
    }

    private bool IsDoorOpen => CurrentState == DoorState.Open;
    private bool IsDoorClosed => CurrentState == DoorState.Closed;

    private void Awake()
    {
        _animator = GetComponent<Animation>();
        if (_animator == null)
        {
            Debug.LogError("Every DoorController needs an Animator.");
            return;
        }

        _animator.playAutomatically = false;

        _openAnimation.legacy = true;
        _closeAnimation.legacy = true;
        _animator.AddClip(_openAnimation, DoorState.Open.ToString());
        _animator.AddClip(_closeAnimation, DoorState.Closed.ToString());
    }

    private void Start()
    {            
        _currentState = _initialState;
        string clip = GetCurrentAnimation();
        _animator[clip].speed = 9999;
        _animator.Play(clip);
    }

    private void CloseDoor()
    {
        if (IsDoorClosed)
        {
            return;
        }

        CurrentState = DoorState.Closed;
        _doorCollider.enabled = true;
    }

    private void OpenDoor()
    {
        if (IsDoorOpen)
        {
            return;
        }

        CurrentState = DoorState.Open;
        _doorCollider.enabled = false;
    }

    public void ToggleDoor()
    {
        if (IsDoorOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }

    private void Animate()
    {
        string clip = GetCurrentAnimation();
        _animator[clip].speed = _animationSpeed;
        _animator.Play(clip);
    }

    private string GetCurrentAnimation()
    {
        return CurrentState.ToString();
    }
}