using UnityEngine;

public interface IPool<T>
{
    T Pull();
    T Pull(Vector3 position);
    T Pull(Vector3 position, Quaternion rotation);
    public GameObject PullGameObject();
    public GameObject PullGameObject(Vector3 position);
    public GameObject PullGameObject(Vector3 position, Quaternion rotation);
    void Push(T obj);
}