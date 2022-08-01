using System;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu()]
public class JumpTrajectory : ScriptableObject
{
    [SerializeField] private AnimationCurve _yMovement;
    [SerializeField] private float _jumpPower = 5f;
    [SerializeField] private float _duration = 1f;

    private Vector3 _currentPosition;
    private Vector3 _endPosition;
    private float Distance => (_endPosition - _currentPosition).magnitude;

    public async void Jump(Transform point, Transform to, Action callback = null)
    {
        _currentPosition = point.position;

        float elapsedTime = 0;
        float percent = 0;

        while (percent <= 1)
        {
            _endPosition = to.position;

            float leftTime = _duration - elapsedTime;
            float speed = Distance / leftTime;
            float step = speed * Time.deltaTime;

            _currentPosition = Vector3.MoveTowards(
                _currentPosition,
                to.position, step);

            percent = elapsedTime / _duration;
            point.position = _currentPosition + Vector3.up * _yMovement.Evaluate(percent) * _jumpPower;

            elapsedTime += Time.deltaTime;

            await Task.Yield();
        }

        callback?.Invoke();
    }
}