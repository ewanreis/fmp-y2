using UnityEngine;

public class WeaponFollow : MonoBehaviour
{
    //* This script makes entity weapons follow the entity
    [SerializeField] private Transform target = null;
    [SerializeField] private float followRadius = 2f;
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private float initialSpeed = 2f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float wobbleSpeed = 10f;
    [SerializeField] private float wobbleMagnitude = 0.2f;

    private Vector3 initialPosition;
    private bool isMoving = false;
    private float wobbleOffset = 0f;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        if(target == null || transform == null)
        {
            Destroy(this.gameObject);
            return;
        }

        MoveTowardsTarget();
        WobbleGameObject();
    }

    private void MoveTowardsTarget()
    {
        Vector3 direction = target.position - transform.position;
        float distance = direction.magnitude;
        float speed = Mathf.Lerp(initialSpeed, maxSpeed, distance / maxDistance);

        Vector3 followPosition = target.position - (direction.normalized * followRadius);

        if (distance <= maxDistance)
        {
            if (distance > followRadius)
            {
                MoveTowards(followPosition, speed);
            }

            else
            {
                isMoving = false;
            }
        }

        else
        {
            MoveTowards(followPosition, speed * 10);
        }
    }

    private void MoveTowards(Vector3 position, float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);
        isMoving = true;
    }

    private void WobbleGameObject()
    {
        if(!isMoving)
            return;

        if (wobbleOffset < 1f)
        {
            wobbleOffset += Time.deltaTime * wobbleSpeed;
            float wobble = Mathf.Lerp(0f, wobbleMagnitude, Mathf.SmoothStep(0f, 1f, wobbleOffset));
            transform.rotation = Quaternion.Euler(0f, 0f, wobble * Mathf.Sin(Time.time * 10f));
        }
        else if (wobbleOffset > 0f)
        {
            wobbleOffset -= Time.deltaTime * wobbleSpeed;
            float wobble = Mathf.Lerp(0f, wobbleMagnitude, Mathf.SmoothStep(0f, 1f, wobbleOffset));
            transform.rotation = Quaternion.Euler(0f, 0f, wobble * Mathf.Sin(Time.time * 10f));
        }
    }
}