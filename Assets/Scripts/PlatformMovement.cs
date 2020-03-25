using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public Vector3[] wayPoints;
    private int _currentWayPointIndex;
    private Vector3 _directionTowardsNextWayPoint;
    private bool _platformStopped;
    private float _wayPointPausInSeconds;
    
    public float travelForce;

    public void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _currentWayPointIndex = 0;
        _platformStopped = false;
        _wayPointPausInSeconds = 4;
    }

    public void Start()
    {
        CalculateDirectionTowardsNextWayPoint();
    }

    public void Update()
    {
        if (PlatformHasReachedWaypoint() && !_platformStopped) StartCoroutine(WayPointMovementPause());
    }

    public void FixedUpdate()
    {
        if (!_platformStopped) MoveTowardsWayPoint();
    }

    private void MoveTowardsWayPoint()
    {
        _rigidbody.velocity = _directionTowardsNextWayPoint * (travelForce * Time.deltaTime);
    }

    private bool PlatformHasReachedWaypoint()
    {
        return Vector3.Distance(transform.position, wayPoints[_currentWayPointIndex]) <= 1f;
    }

    private void OrderPlatformTowardsNextWayPoint()
    {
        _currentWayPointIndex++;
        if (_currentWayPointIndex > wayPoints.Length - 1) _currentWayPointIndex = 0;
        CalculateDirectionTowardsNextWayPoint();
    }

    private void StopPlatformMovement()
    {
        _rigidbody.velocity = Vector3.zero;
        _platformStopped = true;
    }

    private void ContinuePlatformMovement()
    {
        _platformStopped = false;
    }

    private void CalculateDirectionTowardsNextWayPoint()
    {
        _directionTowardsNextWayPoint = (wayPoints[_currentWayPointIndex] - transform.position).normalized;
    }

    private IEnumerator WayPointMovementPause()
    {
        StopPlatformMovement();
        yield return new WaitForSeconds(_wayPointPausInSeconds);
        OrderPlatformTowardsNextWayPoint(); 
        ContinuePlatformMovement();
    }

    private string TooString()
    {
        string str = "_currentWayPointIndex: " + _currentWayPointIndex + "\n";
        str += "_directionTowardsnextWaypoint: " + _directionTowardsNextWayPoint + "\n";
        str += "wayPoints: \n";
        for (int i = 0; i < wayPoints.Length; i++) {
            str += "Index: " + i + " Value: " + wayPoints[i] + "\n";
        } 
        return str;
    }
}
