using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThroneShoot : MonoBehaviour
{
    //* Handles shooting for the throne
    public static event System.Action OnShoot;
    [SerializeField] private GameObject primaryBulletPrefab;
    [SerializeField] private GameObject secondaryBulletPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float primaryBulletSpeed = 100f;
    [SerializeField] private float secondaryBulletSpeed = 50f;
    [SerializeField] private float primaryShootDelay = 0.3f;
    [SerializeField] private float secondaryShootDelay = 1.5f;
    [SerializeField] private Transform projectileParent;

    private bool isShootingPrimary = false;
    private bool isShootingSecondary = false;
    private bool canShoot = false;
    private Camera cam;

    private void OnEnable()
    {
        cam = Camera.main;
        InputManager.OnPrimaryPressed += ShootPrimary;
        InputManager.OnPrimaryHeld += ShootPrimary;

        InputManager.OnSecondaryPressed += ShootSecondary;
        InputManager.OnSecondaryHeld += ShootSecondary;

        MountableObject.OnMount += EnableShoot;
        MountableObject.OnUnmount += DisableShoot;
    }

    private void OnDisable()
    {
        InputManager.OnPrimaryPressed -= ShootPrimary;
        InputManager.OnPrimaryHeld -= ShootPrimary;

        InputManager.OnSecondaryPressed -= ShootSecondary;
        InputManager.OnSecondaryHeld -= ShootSecondary;

        MountableObject.OnMount -= EnableShoot;
        MountableObject.OnUnmount -= DisableShoot;
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
        bullet.transform.parent = projectileParent;

        Vector3 directionPos = new Vector3();
        Vector3 shootDirection = new Vector3();
        if(InputManager.usingController == false)
        {
            // Get the direction from the player to the mouse position
            directionPos = InputManager.GetMousePosition();

            directionPos.z = cam.transform.position.z - transform.position.z;
            shootDirection = -(cam.ScreenToWorldPoint(directionPos) - shootPoint.position);
        }
        else
        {
            directionPos = InputManager.GetRightStickDirection() * 10;
            shootDirection = directionPos;
            Debug.Log("using controller joystick");
            //directionPos.z = cam.transform.position.z - transform.position.z;
        }

        // Normalize the shoot direction and set the bullet velocity
        bullet.GetComponent<Rigidbody2D>().velocity = shootDirection.normalized * primaryBulletSpeed;
        OnShoot.Invoke();
        Destroy(bullet, 5f);
        yield return new WaitForSeconds(primaryShootDelay);
        isShootingPrimary = false;
    }

    private IEnumerator ShootSecondaryCoroutine()
    {
        isShootingSecondary = true;
        // Spawn a new bullet prefab
        GameObject bullet = Instantiate(secondaryBulletPrefab, shootPoint.position, Quaternion.identity);
        bullet.transform.parent = projectileParent;

        Vector3 directionPos = new Vector3();
        Vector3 shootDirection = new Vector3();
        if(InputManager.usingController == false)
        {
            // Get the direction from the player to the mouse position
            directionPos = InputManager.GetMousePosition();

            directionPos.z = cam.transform.position.z - transform.position.z;
            shootDirection = -(cam.ScreenToWorldPoint(directionPos) - shootPoint.position);
        }
        else
        {
            directionPos = InputManager.GetRightStickDirection() * 100;
            shootDirection = directionPos;
            //directionPos.z = cam.transform.position.z - transform.position.z;
        }

        // Normalize the shoot direction and set the bullet velocity
        bullet.GetComponent<Rigidbody2D>().velocity = shootDirection.normalized * secondaryBulletSpeed;
        OnShoot.Invoke();
        Destroy(bullet, 10f);
        yield return new WaitForSeconds(secondaryShootDelay);
        isShootingSecondary = false;
    }
}
