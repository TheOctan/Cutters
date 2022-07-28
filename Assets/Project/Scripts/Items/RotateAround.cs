using System;
using UnityEngine;

[SelectionBase]
public class RotateAround : MonoBehaviour
{
    private enum RotationAxis
    {
        X,
        Y,
        Z
    }

    [SerializeField] private float _speed = 1f;
    [SerializeField] private RotationAxis _rotationAxis = RotationAxis.X;

    private const int ANGLE_PER_SECOND = 360;

    private void Update()
    {
        transform.RotateAround(transform.position, GetRotationAxis(), ANGLE_PER_SECOND * _speed * Time.deltaTime);
    }

    private Vector3 GetRotationAxis()
    {
        switch (_rotationAxis)
        {
            case RotationAxis.X:
                return transform.right;
            case RotationAxis.Y:
                return transform.up;
            case RotationAxis.Z:
                return transform.forward;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}