using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThroneShoot : MonoBehaviour
{
    [SerializeField]
    private GameObject primaryBulletPrefab;
    [SerializeField]
    private GameObject secondaryBulletPrefab;

    [SerializeField]
    private float primaryBulletSpeed = 100f;

    [SerializeField]
    private float secondaryBulletSpeed = 50f;

    [SerializeField]
    private Transform shootPoint;

    [SerializeField]
    private float primaryShootDelay = 0.3f;

    [SerializeField]
    private float secondaryShootDelay = 1.5f;

    private bool isShootingPrimary = false;
    private bool isShootingSecondary = false;
    private bool canShoot = false;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
        InputManager.OnPrimaryPressed += ShootPrimary;
        InputManager.OnPrimaryHeld += ShootPrimary;

        InputManager.OnSecondaryPressed += ShootSecondary;
        InputManager.OnSecondaryHeld += ShootSecondary;

        MountableObject.OnMount += EnableShoot;
        MountableObject.OnUnmount += DisableShoot;
    }

    private void ShootPrimary()
    {
        if(!isShootingPrimary && canShoot && !PauseMenu.paused)
            StartCoroutine(ShootPrimaryCoroutine());
    }

    private void ShootSecondary()
    {
        if(!isShootingSecondary && canShoot && !PauseMenu.paused)
            StartCoroutine(ShootSecondaryCoroutine());
    }

    private void EnableShoot()
    {
        canShoot = true;
    }

    private void DisableShoot()
    {
        canShoot = false;
    }

    private IEnumerator ShootPrimaryCoroutine()
    {
        isShootingPrimary = true;
        // Spawn a new bullet prefab
        GameObject bullet = Instantiate(primaryBulletPrefab, shootPoint.position, Quaternion.identity);

        // Get the direction from the player to the mouse position
        Vector3 mousePos = InputManager.GetMousePosition();

        mousePos.z = cam.transform.position.z - transform.position.z;
        Vector3 shootDirection = -(cam.ScreenToWorldPoint(mousePos) - shootPoint.position);

        // Normalize the shoot direction and set the bullet velocity
        bullet.GetComponent<Rigidbody2D>().velocity = shootDirection.normalized * primaryBulletSpeed;

        Destroy(bullet, 5f);
        yield return new WaitForSeconds(primaryShootDelay);
        isShootingPrimary = false;
    }

    private IEnumerator ShootSecondaryCoroutine()
    {
        isShootingSecondary = true;
        // Spawn a new bullet prefab
        GameObject bullet = Instantiate(secondaryBulletPrefab, shootPoint.position, Quaternion.identity);

        // Get the direction from the player to the mouse position
        Vector3 mousePos = InputManager.GetMousePosition();

        mousePos.z = cam.transform.position.z - transform.position.z;
        Vector3 shootDirection = -(cam.ScreenToWorldPoint(mousePos) - shootPoint.position);

        // Normalize the shoot direction and set the bullet velocity
        bullet.GetComponent<Rigidbody2D>().velocity = shootDirection.normalized * secondaryBulletSpeed;

        Destroy(bullet, 10f);
        yield return new WaitForSeconds(secondaryShootDelay);
        isShootingSecondary = false;
    }
}
