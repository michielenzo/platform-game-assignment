using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class EnemyController : MonoBehaviour
{
    public EnemyConfig _EnemyConfig;
    public GameObject playerObject;

    public GameObject gravityInversionFieldReference;
    
    private Rigidbody _rigidBody;

    private bool _summonGravityInversionFieldLock;
    public int _summoningReloadTime;
    public int gravityInversionFieldLifetime;

    public int currentHealth { get; set; }

    private enum States { Idle, Chasing, Summoning }
    private States currentState;

    public void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        currentHealth = _EnemyConfig.MaxHealth;
        currentState = States.Idle;
        _summonGravityInversionFieldLock = false;
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

        if (IsPlayerInSummoningRange()) currentState = States.Summoning;
        if (!IsPlayerInChasingRange()) currentState = States.Idle;
    }

    private void Summoning()
    {
        _rigidBody.velocity = Vector3.zero;
        if(!_summonGravityInversionFieldLock) SummonGravityInversionField();
        if (!IsPlayerInSummoningRange()) currentState = States.Chasing;
    }

    private void SummonGravityInversionField()
    {
        // Fire a raycast to determine the Y axis of the gravity inversion field.
        RaycastHit hit = new RaycastHit();
        if (!Physics.Raycast(transform.position, Vector3.down, out hit)) return;
        float gravityInversionFieldYAxis = hit.transform.position.y;
        
        // Determine the location of the gravity inversion field.
        Vector3 gravityInversionFieldLocation = playerObject.transform.position;
        gravityInversionFieldLocation.y = gravityInversionFieldYAxis + 14;
        
        GameObject gravityInversionField = Instantiate(gravityInversionFieldReference, 
                                                       gravityInversionFieldLocation, 
                                                       Quaternion.identity);
        _summonGravityInversionFieldLock = true;
        
        StartCoroutine(SummonLockResetTimer());
        StartCoroutine(DestroyAfterTime(gravityInversionField));
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
    
    private IEnumerator SummonLockResetTimer()
    {
        yield return new WaitForSeconds(_summoningReloadTime);
        _summonGravityInversionFieldLock = false;
    }
    
    private IEnumerator DestroyAfterTime(Object gameObject)
    {
        yield return new WaitForSeconds(gravityInversionFieldLifetime);
        Destroy(gameObject);
    }
}
