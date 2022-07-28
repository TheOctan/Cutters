using DG.Tweening;
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

        objects[1].transform.parent = transform.parent;
        GameObject obj = objects[0];
        var collider1 = obj.AddComponent<BoxCollider>();
        collider1.enabled = false;
        obj.layer = LayerMask.NameToLayer("Field");
        obj.transform.DOJump(obj.transform.position, 2f, 1, 0.75f).SetEase(Ease.InOutSine);
        obj.transform.DOScale(Vector3.one * 0.3f, 0.75f)
            .SetEase(Ease.InOutSine)
            .OnComplete(() => collider1.enabled = true);
        obj.AddComponent<StackableSheaf>();

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