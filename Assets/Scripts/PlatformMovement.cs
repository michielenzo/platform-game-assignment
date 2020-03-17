using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    private Rigidbody _rigidbody;
        
    public float travelDistance;
    public Vector3 travelDirection;
    private Vector3 _startingPosition;
    public float travelForce;

    private bool _isDeparting;
    private bool _isDepartingWriteLock;
    
    public void Awake()
    {
        _isDeparting = true;
        _isDepartingWriteLock = true;
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Start()
    {
        _startingPosition = transform.position;
    }

    public void Update()
    {
        Debug.Log(TooString());
        
        Vector3 currentPosition = transform.position;
        Vector3 destination = _startingPosition + travelDirection.normalized * travelDistance;
        
        if (IsPlatformOutsideTravellingBounds(currentPosition, destination) && !_isDepartingWriteLock) {
            _isDeparting = !_isDeparting;
            _isDepartingWriteLock = true;
            Debug.Log("here");
        } else if (!IsPlatformOutsideTravellingBounds(currentPosition, destination)){
            _isDepartingWriteLock = false;
        }
    }

    public void FixedUpdate()
    {
        Vector3 travellingForce = _isDeparting ?
            travelDirection.normalized * (travelForce * Time.deltaTime):
            -travelDirection.normalized * (travelForce * Time.deltaTime);
        _rigidbody.AddForce(travellingForce);
    }
    
    private bool IsPlatformOutsideTravellingBounds(Vector3 currentPosition, Vector3 destination)
    {
        //check for x,y,z if the destination is positive/negative relative to the startingPosition.
        bool xPositive = destination.x > _startingPosition.x;
        bool yPositive = destination.y > _startingPosition.y;
        bool zPositive = destination.z > _startingPosition.z;

        //check if x,y,z are outside the travelling bounds. 
        bool xOutsideBounds = isAxisOutOfBounds(currentPosition.x, destination.x, _startingPosition.x, xPositive, travelDirection.x);
        bool yOutsideBounds = isAxisOutOfBounds(currentPosition.y, destination.y, _startingPosition.y, yPositive, travelDirection.y);;
        bool zOutsideBounds = isAxisOutOfBounds(currentPosition.z, destination.z, _startingPosition.z, zPositive, travelDirection.z);;

        return xOutsideBounds && yOutsideBounds && zOutsideBounds;
    }

    private static bool isAxisOutOfBounds(float currentpositionAxis, float destinationAxis, float startingPositionAxis, bool axisPositive, float travelDirection)
    {
        if (travelDirection == 0) return true;
        if (axisPositive) {
            return currentpositionAxis > destinationAxis || currentpositionAxis < startingPositionAxis;
        }
        return currentpositionAxis < destinationAxis || currentpositionAxis > startingPositionAxis;
    }

    private string TooString()
    {
        Vector3 destination = _startingPosition + travelDirection.normalized * travelDistance;
        string str = "";
        str += "travelDistance: " + travelDistance;
        str += " travelDirection: " + travelDirection;
        str += " _startingPosition: " + _startingPosition;
        str += " destination: " + destination;
        str += " travelForce: " + travelForce;
        str += " _isDeparting: " + _isDeparting;
        str += " _isDepartingWriteLock: " + _isDepartingWriteLock;
        return str;
    }
}
