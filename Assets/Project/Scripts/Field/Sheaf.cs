using EzySlice;
using UnityEngine;

public class Sheaf : MonoBehaviour, IDestroyable
{
    [SerializeField] private Collider _collider;
    [SerializeField] private Renderer _renderer;

    private Material _material;

    private void Awake()
    {
        _material = _renderer.material;
    }

    public void Destroy()
    {
        _collider.enabled = false;
        GameObject[] objects = Slice(GetSlicePosition(), Vector3.up);
        for (var i = 0; i < objects.Length; i++)
        {
            objects[i].transform.parent = transform.parent;
        }

        GameObject obj = objects[0];
        obj.AddComponent<BoxCollider>();
        var rb = obj.AddComponent<Rigidbody>();
        rb.AddForce(Vector3.up * 5f, ForceMode.VelocityChange);
        obj.layer = LayerMask.NameToLayer("Field");

        gameObject.SetActive(false);
    }

    private GameObject[] Slice(Vector3 planeWorldPosition, Vector3 planeWorldDirection)
    {
        return gameObject.SliceInstantiate(planeWorldPosition, planeWorldDirection, _material);
    }

    private Vector3 GetSlicePosition()
    {
        Vector3 halfHeight = transform.localScale / 2f;
        return transform.position - halfHeight * 0.9f;
    }
}