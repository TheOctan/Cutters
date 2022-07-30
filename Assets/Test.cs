using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private Transform _uiPoint;
    [SerializeField] private Camera _camera;

    private void Update()
    {
        Vector3 world = _camera.ScreenToWorldPoint(_uiPoint.position);
        var local = new Vector3(world.x, world.y, transform.position.z);
        transform.position = local;
    }
}