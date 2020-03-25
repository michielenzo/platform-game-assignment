
using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float rotationSpeed;
    private Vector3 _rotation;
    private Rigidbody _rigidBody;
    public float pushBackForceMultiplier;

    private void Awake()
    {
        _rotation = new Vector3(1,1,1);
        _rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    public void Update()
    {
        Rotate();
    }
    
    private void Rotate() 
    {
        transform.Rotate(_rotation * (rotationSpeed * Time.deltaTime));
    }

    private void OnCollisionEnter(Collision other)
    {
        if (ColliderIsAnEnemy(other)) {
            ReduceEnemyHealth(other);
            Destroy(gameObject);
            if (IsEnemyStillAlive(other)) PushBackEnemy(other);
            else Destroy(other.gameObject);           
        }
    }

    private static bool ColliderIsAnEnemy(Collision other)
    {
        return other.transform.CompareTag("Enemy");
    }

    private static void ReduceEnemyHealth(Collision other)
    {
        other.gameObject.GetComponent<EnemyController>().currentHealth--;
    }

    private static bool IsEnemyStillAlive(Collision other)
    {
        return other.gameObject.GetComponent<EnemyController>().currentHealth > 0;
    }

    private void PushBackEnemy(Collision other)
    {
        Rigidbody colliderRigidBody = other.gameObject.GetComponent<Rigidbody>();
        Vector3 pushBackForce = _rigidBody.velocity.normalized * pushBackForceMultiplier;
        colliderRigidBody.AddForce(pushBackForce);
    }
}
