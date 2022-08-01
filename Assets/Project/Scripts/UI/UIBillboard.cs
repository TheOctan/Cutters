using UnityEngine;

[ExecuteAlways]
[AddComponentMenu("Layout/UI Billboard")]
public class UIBillboard : MonoBehaviour
{
    [SerializeField] private bool _alignAnchor = true;
    private Transform _cameraTransform;

    private void OnEnable()
    {
        _cameraTransform = Camera.main.transform;
    }

    private void OnDisable()
    {
        _cameraTransform = null;
    }

    private void LateUpdate()
    {
        if (_cameraTransform)
        {
            Vector3 lookDirection = transform.position + _cameraTransform.forward;
            if (_alignAnchor)
            {
                transform.LookAt(lookDirection, _cameraTransform.up);
            }
            else
            {
                transform.LookAt(lookDirection);
            }
        }
    }
}