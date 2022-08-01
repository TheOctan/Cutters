using System;
using System.Threading.Tasks;
using DG.Tweening;
using EzySlice;
using UnityEngine;

public class Sheaf : MonoBehaviour, IDestroyable
{
    public event Action OnDestroyed;

    [SerializeField] private Renderer _renderer;
    [SerializeField] private Collider _collider;
    [Header("Properties")]
    [SerializeField] private float _growthDuration = 0.3f;

    private Material _material;
    private GameObject _base;

    public bool IsDestroyed { get; private set; }

    private void Awake()
    {
        _material = _renderer.material;
    }

    public void Grow()
    {
        GrowAnimate(0f);
    }

    public void Grow(float delay)
    {
        GrowAnimate(delay);
    }

    public void Grow(float newHeight, float delay)
    {
        RecalculatePosition(newHeight);
        RecalculateScale(newHeight);

        GrowAnimate(delay);
    }

    private void RecalculateScale(float newHeight)
    {
        Vector3 scale = transform.localScale;
        scale.y = newHeight;
        transform.localScale = scale;
    }

    private void RecalculatePosition(float newHeight)
    {
        Vector3 position = transform.localPosition;
        position.y = newHeight * 0.5f;
        transform.localPosition = position;
    }

    private async void GrowAnimate(float delay)
    {
        gameObject.SetActive(true);
        Vector3 originalScale = transform.localScale;
        _collider.enabled = false;
        transform.localScale = Vector3.zero;

        await Task.Delay((int)(1000 * delay));

        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(originalScale, _growthDuration))
            .Append(
                transform.DOShakeScale(0.2f, 
                    (Vector3.forward + Vector3.right) * 0.2f,
                    1, 0))
            .OnComplete(() =>
            {
                transform.localScale = originalScale;
                _collider.enabled = true;
                IsDestroyed = false;
                Destroy(_base);
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
    }

    private void InitBase(GameObject obj)
    {
        _base = obj;
        _base.transform.SetParent(transform.parent, false);
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