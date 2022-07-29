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

    public void Grow()
    {
        
    }

    public void Destroy()
    {
        // TODO: confuse with global positions
        GameObject[] gameObjects = Slice(GetSlicePosition(), Vector3.up);

        gameObjects[1].transform.parent = transform.parent;
        CreateStackableSheaf(gameObjects[0]);

        _collider.enabled = false;
        gameObject.SetActive(false);
    }

    private static void CreateStackableSheaf(GameObject gameObject)
    {
        gameObject.layer = LayerMask.NameToLayer("Field");
        gameObject.AddComponent<BoxCollider>().enabled = false;
        var stackableSheaf = gameObject.AddComponent<StackableSheaf>();
        stackableSheaf.Animate();
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