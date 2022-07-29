using System;
using UnityEngine;

public class Coin : MonoBehaviour, IPoolable<Coin>
{
    private Action<Coin> _returnToPool;

    private void OnDisable()
    {
        ReturnToPool();
    }

    void IPoolable<Coin>.Initialize(Action<Coin> callback)
    {
        _returnToPool = callback;
    }

    public void ReturnToPool()
    {
        _returnToPool?.Invoke(this);
    }
}