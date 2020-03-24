using System;
using System.Collections.Generic;
using UnityEngine;

public class GravityInversionFieldController : MonoBehaviour
{

    private List<Rigidbody> _rigidBodies;

    private Vector3 _gravityDirection;

    public float _forceMultiplier;

    private void Awake()
    {
        _rigidBodies = new List<Rigidbody>();
        _gravityDirection = Vector3.up;
    }

    private void FixedUpdate()
    {
        foreach (var rigidBody in _rigidBodies) {
            if (rigidBody == null) {
                _rigidBodies.Remove(rigidBody);
                return;
            }
            
            InvertGravity(rigidBody);
        }
    }

    private void InvertGravity(Rigidbody rigidBody)
    {
        rigidBody.AddForce(_gravityDirection * (_forceMultiplier * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rigidBody = other.GetComponent<Rigidbody>();
        if (rigidBody == null) return;
        rigidBody.useGravity = false;
        _rigidBodies.Add(rigidBody);
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody rigidBody = other.GetComponent<Rigidbody>();
        if (rigidBody == null) return;
        rigidBody.useGravity = true;
        _rigidBodies.Remove(rigidBody);
    }
}
