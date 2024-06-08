using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 1.0f;
    public float bulletSpeed = 10.0f;
    public float bulletLifetime = 5.0f;
    public LayerMask obstacleLayer;
    public float maxRayDistance = 100.0f;

    private float nextFireTime = 0.0f;

    void Update()
    {
        if (Time.time >= nextFireTime && IsPathClear())
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    bool IsPathClear()
    {
        RaycastHit hit;
        bool pathClear = !Physics.Raycast(firePoint.position, firePoint.forward, out hit, maxRayDistance, obstacleLayer);
        return pathClear;
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        rb.velocity = transform.right * bulletSpeed /2; // Asigna velocidad hacia adelante

        Destroy(bullet, bulletLifetime);
    }
}








