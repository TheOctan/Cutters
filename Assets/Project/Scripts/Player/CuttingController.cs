using System;
using Project.Scripts.Field;
using UnityEngine;

public class CuttingController : MonoBehaviour
{
    [SerializeField] private float _castRadius = 1f;
    [SerializeField] private float _castHeight = 0.5f;
    [SerializeField] private LayerMask _layerMask;

    private IPlayerController _playerController;

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

    private void Raycast()
    {
        Vector3 position = transform.position;
        position.y = _castHeight;

        var ray = new Ray(position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, _castRadius, _layerMask))
        {
            if (hit.collider.TryGetComponent(out IDestroyable obj))
            {
                obj.Destroy();
            }
        }
    }
}