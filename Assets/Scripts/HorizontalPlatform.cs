using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalPlatform : MonoBehaviour
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
        //check for x,y,z if the destination is positive/negative relative to the destination.
        bool xPositive = destination.x > _startingPosition.x;
        bool yPositive = destination.y > _startingPosition.y;
        bool zPositive = destination.z > _startingPosition.z;

        //check if x,y,z are outside the travelling bounds.


        bool xOutsideBounds;
        if (travelDirection.x == 0) {
            xOutsideBounds = true;
        } else {
            if (xPositive) {
                xOutsideBounds = currentPosition.x > destination.x || currentPosition.x < _startingPosition.x;
            }else {
                xOutsideBounds = currentPosition.x < destination.x || currentPosition.x > _startingPosition.x;
            }           
        }
        
        bool yOutsideBounds;
        if (travelDirection.y == 0) {
            yOutsideBounds = true;
        } else {
            if (yPositive) {
                yOutsideBounds = currentPosition.y > destination.y || currentPosition.y < _startingPosition.y;
            }else {
                yOutsideBounds = currentPosition.y < destination.y || currentPosition.y > _startingPosition.y;
            }
        }

        bool zOutsideBounds;
        if (travelDirection.z == 0) {
            zOutsideBounds = true;
        } else {
            if (zPositive) {
                zOutsideBounds = currentPosition.z > destination.z || currentPosition.z < _startingPosition.z;
            } else {
                zOutsideBounds = currentPosition.z < destination.z || currentPosition.z > _startingPosition.z;
            }                    
        }
        
        Debug.Log("xOuts: " + xOutsideBounds + " yOuts: " + yOutsideBounds + " zOuts: " + zOutsideBounds);

        return xOutsideBounds && yOutsideBounds && zOutsideBounds;
    }

    public string TooString()
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
