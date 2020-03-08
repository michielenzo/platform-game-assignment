using System.Collections;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public GameObject bulletObj;
    public float bulletSpeed;
    public int maxBullets;
    public float bulletLifeTime;
    private int _bulletsInWorld;
    public float bulletSpawnOffset;

    private bool _fireLock;

    public void Awake()
    {
        _bulletsInWorld = 0;
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
        if (_bulletsInWorld >= maxBullets) return;
        var transform1 = transform;
        var forward = transform1.forward;
        Vector3 direction = forward.normalized;
        Vector3 offset = forward.normalized * bulletSpawnOffset;
        GameObject bullet = Instantiate(bulletObj, transform1.position + offset, Quaternion.identity);
        _bulletsInWorld++;
        bullet.gameObject.GetComponent<Rigidbody>().AddForce(direction * (bulletSpeed * Time.deltaTime));
        StartCoroutine(DestroyAfterTime(bullet));
    }
    
    private IEnumerator DestroyAfterTime(GameObject gameObject)
    {
        yield return new WaitForSeconds(bulletLifeTime);
        Destroy(gameObject);
        _bulletsInWorld--;
    }
}