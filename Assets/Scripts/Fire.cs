using System;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{

    public GameObject bulletObj;

    public float bulletLifeSpan;
    public float bulletSpeed;
    public int maxBullets;

    public Vector3 bulletSpawnOffset;
    
    private int _bulletsInWorld;

    private bool _fireLock;

    private List<GameObject> _bullets;

    public void Awake()
    {
        _bullets = new List<GameObject>();
        _fireLock = false;
    }

    public void Update()
    {
        //You will only fire again if the fire button was released again.
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (Input.GetAxis("Fire") == 1) {
            if (_fireLock) return;
            FireBullet();
            _fireLock = true;
        } else {
            _fireLock = false;
        }
    }

    private void FireBullet()
    {
        if (_bullets.Count >= maxBullets) return;
        Vector3 direction = transform.forward.normalized;
        GameObject bullet = Instantiate(bulletObj, transform.position + bulletSpawnOffset, Quaternion.identity);
        _bullets.Add(bullet);
        bullet.gameObject.GetComponent<Rigidbody>().AddForce(direction * (bulletSpeed * Time.deltaTime));
    }
}
