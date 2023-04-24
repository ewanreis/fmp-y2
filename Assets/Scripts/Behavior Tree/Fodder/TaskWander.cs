using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class TaskWander : Node
{
    public static event System.Action<EnemyPositionType> OnReachPoint;
    private Transform _transform;
    private Transform _destinationPoint;
    private Vector2 destination;
    private float wanderDistance = 15f;
    private float groundPadding = 1f;
    private LayerMask _groundLayer; // Layer mask for the ground
    private float _walkSpeed;
    private EnemyTypes _type;

    private float _waitTime = 1f;
    private float _waitCounter = 0f;
    private bool _waiting = false;

    public TaskWander(Transform transform, LayerMask groundLayer, float walkSpeed, Transform targetTransform, EnemyTypes enemyType)
    {
        _transform = transform;
        _groundLayer = groundLayer;
        _walkSpeed = walkSpeed;
        _destinationPoint = targetTransform;
        _type = enemyType;
    }

    public override NodeState Evaluate()
    {
        if(_waiting)
            CheckWait();
        
        else
        {
            
            

            if(Vector3.Distance(_transform.position, _destinationPoint.position) < 0.01f)
            {
                //Debug.Log($"{_transform.position},{_destinationPoint.position} Next Point");
                MoveToNextPoint();
            }
            else
            {
                //Debug.Log("Moving");
                MoveToTargetPoint();
            }

        }

        state = NodeState.Running;
        return state;
    }

    private void MoveToNextPoint()
    {
        _transform.position = _destinationPoint.position;
        _waitCounter = 0f;
        EnemyPositionType ePT = new EnemyPositionType();
        ePT.enemyPosition = _transform;
        ePT.type = _type;
        OnReachPoint?.Invoke(ePT);
        _waiting = true;
        destination = GetRandomGroundedPoint();
        if(destination.x < -115)
            destination.x = -115;

        if(destination.x > 115)
            destination.x = 115;
        //Debug.Log(destination);
        _destinationPoint.position = new Vector3(destination.x, destination.y, 0);
    }

    private void MoveToTargetPoint()
    {
        _transform.position = Vector3.MoveTowards(_transform.position, _destinationPoint.position, _walkSpeed * Time.deltaTime);
    }

    private Vector2 GetRandomGroundedPoint()
    {
        // Get a random x coordinate within the screen width
        float randomX = Random.Range(-wanderDistance + _transform.position.x, wanderDistance + _transform.position.x);

        

        RaycastHit2D hit = Physics2D.Raycast(new Vector2(randomX, _transform.position.y + 100f), Vector2.down, Mathf.Infinity, _groundLayer);

        // If the raycast hits something, add the ground Y coordinate to the hit point's y coordinate to ground the point
        float randomY = _transform.position.y;
        if (hit.collider != null)
        {
            randomY = hit.point.y + groundPadding;
        }

        // Return the random grounded point as a Vector2
        return new Vector2(randomX, randomY);
    }

    private void CheckWait()
    {
        _waitCounter += Time.deltaTime;

        if(_waitCounter >= _waitTime)
            _waiting = false;
    }
}

public struct EnemyPositionType
{
    public Transform enemyPosition;
    public EnemyTypes type;
}
