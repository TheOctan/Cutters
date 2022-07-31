using System;
using UnityEngine;

public class CuttingController : MonoBehaviour
{
    [SerializeField] private float _castDistance = 1f;
    [SerializeField] private float _castRadius = 0.3f;
    [SerializeField] private float _castHeight = 0.5f;
    [SerializeField] private LayerMask _layerMask;

    [SerializeField] private GameObject _raycastedObject;
    

    private IPlayerController _playerController;
    private bool _isCast;

    private void Awake()
    {
        _playerController = GetComponent<IPlayerController>();
    }

    private void OnEnable()
    {
        _playerController.OnAttack += OnAttackHandler;
    }

    private void OnDisable()
    {
        _playerController.OnAttack -= OnAttackHandler;
    }

    private void OnAttackHandler()
    {
        Raycast();
    }

    private void Update()
    {
        Raycast();
    }

    private void Raycast()
    {
        Vector3 position = transform.position + Vector3.up * _castHeight;
        var ray = new Ray(position, transform.forward);

        if (Physics.SphereCast(ray, _castRadius, out RaycastHit hit, _castDistance, _layerMask))
        {
            if (hit.collider.TryGetComponent(out IDestroyable obj))
            {
                // obj.Destroy();
                _isCast = true;
                _raycastedObject = hit.transform.gameObject;
            }
            else
            {
                _isCast = false;
                _raycastedObject = null;
            }
        }
        else
        {
            _isCast = false;
            _raycastedObject = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 position = transform.position + Vector3.up * _castHeight;
        Vector3 direction = transform.forward;

        Gizmos.color = _isCast ? Color.red : Color.green;

        Gizmos.DrawWireSphere(position + direction * _castDistance, _castRadius);
        Gizmos.DrawRay(position, direction * _castDistance);
    }
}