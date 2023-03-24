using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThroneShoot : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;
    private float bulletSpeed = 100f;

    [SerializeField]
    private Transform shootPoint;

    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
        InputManager.OnPrimaryPressed += Shoot;
    }

    private void Shoot()
    {
        // Spawn a new bullet prefab
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);

        // Get the direction from the player to the mouse position
        Vector3 mousePos = InputManager.GetMousePosition();

        mousePos.z = cam.transform.position.z - transform.position.z;
        Vector3 shootDirection = -(cam.ScreenToWorldPoint(mousePos) - shootPoint.position);

        // Normalize the shoot direction and set the bullet velocity
        bullet.GetComponent<Rigidbody2D>().velocity = shootDirection.normalized * bulletSpeed;
    }
}
