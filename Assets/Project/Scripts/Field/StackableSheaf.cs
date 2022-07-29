using System;
using DG.Tweening;
using UnityEngine;

public class StackableSheaf : MonoBehaviour, IStackable
{
    private Collider _collider;
    
    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    public void Animate()
    {
        transform.DOJump(transform.position, 2f, 1, 0.75f).SetEase(Ease.InOutSine);
        transform.DOScale(Vector3.one * 0.3f, 0.75f)
            .SetEase(Ease.InOutSine)
            .OnComplete(() => _collider.enabled = true);
    }

    public Transform Stack()
    {
        _collider.enabled = false;

        return transform;
    }
}