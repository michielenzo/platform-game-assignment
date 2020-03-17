
using System;
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
        Move();
    }
    
    private void Move() 
    {
        transform.Rotate(_rotation * (rotationSpeed * Time.deltaTime));
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Hit!");
        if (!other.transform.CompareTag("Enemy")) return;
        other.gameObject.GetComponent<EnemyController>().currentHealth--;
        Destroy(gameObject);
        if (other.gameObject.GetComponent<EnemyController>().currentHealth > 0) return;
        Destroy(other.gameObject);
    }
}
