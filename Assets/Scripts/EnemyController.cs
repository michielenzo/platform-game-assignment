using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyConfig _EnemyConfig;
    public GameObject playerObject;
    private Rigidbody _rigidBody;

    public int currentHealth { get; set; }

    private enum States { Idle, Chasing, Summoning }
    private States currentState;

    public void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        currentHealth = _EnemyConfig.MaxHealth;
        currentState = States.Idle;
    }

    public void Update()
    {
        switch (currentState)
        {
            case States.Idle: Idle(); break;
            case States.Chasing: Chasing(); break;
            case States.Summoning: Summoning(); break;
            default: throw new ArgumentOutOfRangeException();
        }
    }

    private void Idle()
    {
        if (IsPlayerInChasingRange()) {
            currentState = States.Chasing;
        }
    }

    private void Chasing()
    {
        RotateTowardsPlayer();
        MoveTowardsPlayer();

        if (IsPlayerInSummoningRange()) {
            currentState = States.Summoning;
        }
    }

    private void Summoning()
    {
        Debug.Log("Summoning!");
    }

    private bool IsPlayerInChasingRange()
    {
        float distanceTowardsPlayer = Vector3.Distance(transform.position, playerObject.transform.position);
        return distanceTowardsPlayer <= _EnemyConfig.chaseDistance;
    }
    
    private bool IsPlayerInSummoningRange()
    {
        float distanceTowardsPlayer = Vector3.Distance(transform.position, playerObject.transform.position);
        return distanceTowardsPlayer <= _EnemyConfig.targetLockDistance;
    }

    private void RotateTowardsPlayer()
    {
        Vector3 distance = playerObject.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(distance);
    }

    private void MoveTowardsPlayer()
    {
        Vector3 distanceTowardsPlayer = playerObject.transform.position - transform.position;
        Vector3 DirectiontowardsPlayer = distanceTowardsPlayer.normalized;
        _rigidBody.AddForce(DirectiontowardsPlayer * (Time.deltaTime * _EnemyConfig.movementForceFactor));
    }
}
