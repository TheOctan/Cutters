using System;
using System.Threading.Tasks;
using DG.Tweening;
using EzySlice;
using UnityEngine;

public class Sheaf : MonoBehaviour, IDestroyable
{
    public event Action OnDestroyed;

    [SerializeField] private Renderer _renderer;
    [Header("Properties")]
    [SerializeField] private float _growDuration = 0.3f;

    private Material _material;
    private GameObject _base;

    public bool IsDestroyed { get; private set; }

    private void Awake()
    {
        _material = _renderer.material;
    }

    public void Grow()
    {
        GrowAnimate(transform.localScale.y, 0f);
    }

    public void Grow(float delay)
    {
        GrowAnimate(transform.localScale.y, delay);
    }

    public void Grow(float newHeight, float delay)
    {
        GrowAnimate(newHeight, delay);
    }

    private async void GrowAnimate(float newHeight, float delay)
    {
        gameObject.SetActive(true);
        Vector3 originalScale = transform.localScale;
        transform.localScale = Vector3.zero;

        await Task.Delay((int)(1000 * delay));

        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(originalScale, _growDuration))
            .Append(
                transform.DOShakeScale(0.2f, 
                    (Vector3.forward + Vector3.right) * 0.2f,
                    1, 0))
            .OnComplete(() =>
            {
                Destroy(_base);
                transform.localScale = originalScale;
                IsDestroyed = false;
            });
    }


    public void Destroy()
    {
        if (IsDestroyed)
        {
            return;
        }
        IsDestroyed = true;

        // TODO: confuse with global positions
        GameObject[] gameObjects = Slice(GetSlicePosition(), Vector3.up);
        CreateStackableSheaf(gameObjects[0]);
        InitBase(gameObjects[1]);

        gameObject.SetActive(false);

        OnDestroyed?.Invoke();

        Grow(5f);
    }

    private void InitBase(GameObject obj)
    {
        _base = obj;
        _base.transform.parent = transform.parent;
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