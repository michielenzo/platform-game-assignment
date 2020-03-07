using UnityEngine;

public class Fire : MonoBehaviour
{

    public GameObject bulletObj;

    public float bulletLifeSpan;
    public float bulletSpeed;

    public Vector3 bulletSpawnOffset;
    
    private int _bulletsInWorld;

    private bool _fireLock = false;

    public void Update()
    {
        //You will only fire again if the fire button was released again.
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
        Vector3 direction = transform.forward.normalized;
        GameObject bullet = Instantiate(bulletObj, transform.position + bulletSpawnOffset, Quaternion.identity);
        bullet.gameObject.GetComponent<Rigidbody>().AddForce(direction * (bulletSpeed * Time.deltaTime));
    }
}
