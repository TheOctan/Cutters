using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class ObjectPool<T> : IPool<T> where T : MonoBehaviour, IPoolable<T>
{
    private readonly Action<T> _pullObject;
    private readonly Action<T> _pushObject;
    private readonly T _prefab;

    private readonly Queue<T> _polledObjects = new Queue<T>();

    public int Count => _polledObjects.Count;

    public ObjectPool(T prefab, int preSpawnCount = 0)
    {
        _prefab = prefab;
        Spawn(preSpawnCount);
    }

    public ObjectPool(T prefab, Action<T> pullObject, Action<T> pushObject, int preSpawnCount = 0) 
        : this(prefab, preSpawnCount)
    {
        _pullObject = pullObject;
        _pushObject = pushObject;
    }

    private void Spawn(int preSpawnCount)
    {
        for (var i = 0; i < preSpawnCount; i++)
        {
            T obj = Object.Instantiate(_prefab);
            obj.name = $"{_prefab.name}_{i}";
            obj.gameObject.SetActive(false);
            _polledObjects.Enqueue(obj);
        }
    }

    public T Pull()
    {
        T obj = Count > 0
            ? _polledObjects.Dequeue()
            : Object.Instantiate(_prefab);

        obj.gameObject.SetActive(true);
        obj.Initialize(Push);

        _pullObject?.Invoke(obj);

        return obj;
    }

    public T Pull(Vector3 position)
    {
        T obj = Pull();
        obj.transform.position = position;
        return obj;
    }

    public T Pull(Vector3 position, Quaternion rotation)
    {
        T obj = Pull();

        Transform transform = obj.transform;
        transform.position = position;
        transform.rotation = rotation;

        return obj;
    }

    public GameObject PullGameObject()
    {
        return Pull().gameObject;
    }

    public GameObject PullGameObject(Vector3 position)
    {
        return Pull(position).gameObject;
    }

    public GameObject PullGameObject(Vector3 position, Quaternion rotation)
    {
        return Pull(position, rotation).gameObject;
    }

    public void Push(T obj)
    {
        _polledObjects.Enqueue(obj);

        _pushObject?.Invoke(obj);
        obj.gameObject.SetActive(false);
    }
}