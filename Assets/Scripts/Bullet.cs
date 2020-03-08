using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float rotationSpeed;

    private Vector3 _rotation;

    private void Awake()
    {
        _rotation = new Vector3(1,1,1);
    }

    // Update is called once per frame
    public void Update()
    {
        transform.Rotate(_rotation * (rotationSpeed * Time.deltaTime));
    }
}
