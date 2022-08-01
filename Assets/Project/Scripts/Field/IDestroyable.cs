using System;

public interface IDestroyable
{
    event Action OnDestroyed;
    void Destroy();
    bool IsDestroyed { get; }
}