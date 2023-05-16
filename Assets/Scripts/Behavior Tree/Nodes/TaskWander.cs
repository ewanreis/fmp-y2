using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class TaskWander : Node
{
    public static event System.Action OnReachPoint;
    private Transform _transform;
    private Transform _destinationPoint;
    private Vector2 destination;
    private float wanderDistance = 15f;
    private float groundPadding = 2f;
    private LayerMask _groundLayer; // layer mask for the ground
    private float _walkSpeed;
    private EnemyTypes _type;

    private float _waitTime = 1f;
    private float _waitCounter = 0f;
    private bool _waiting = false;

    public TaskWander(Transform transform, LayerMask groundLayer, float walkSpeed, Transform targetTransform)
    {
        _transform = transform;
        _groundLayer = groundLayer;
        _walkSpeed = walkSpeed;
        _destinationPoint = targetTransform;
    }

    public override NodeState Evaluate()
    {
        if(_waiting)
            CheckWait();

        else
        {
            if(Vector3.Distance(_transform.position, _destinationPoint.position) < 1f)
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
        //_transform.position = _destinationPoint.position;
        _waitCounter = 0f;

        OnReachPoint?.Invoke();
        _waiting = true;
        destination = GetRandomGroundedPoint();
        if(destination.x < -115)
            destination.x = -115;

        if(destination.x > 115)
            destination.x = 115;

        _destinationPoint.position = new Vector3(destination.x, destination.y, 0);
    }

    private void MoveToTargetPoint()
    {
        _transform.position = Vector3.MoveTowards(_transform.position, _destinationPoint.position, _walkSpeed * Time.deltaTime);
        //Debug.DrawLine(_transform.position, _destinationPoint.position, Color.green);
    }

    private Vector2 GetRandomGroundedPoint()
    {
        // get random x coordinate
        float randomX = Random.Range(-wanderDistance + _transform.position.x, wanderDistance + _transform.position.x);

        

        RaycastHit2D hit = Physics2D.Raycast(new Vector2(randomX, _transform.position.y + 100f), Vector2.down, Mathf.Infinity, _groundLayer);

        // if raycast hits something, add ground Y coordinate to the hit point's y coordinate to ground the point
        float randomY = _transform.position.y;
        if (hit.collider != null)
        {
            randomY = hit.point.y + groundPadding;
        }

        // return random grounded point as a Vector2
        return new Vector2(randomX, randomY);
    }

    private void CheckWait()
    {
        _waitCounter += Time.deltaTime;

        if(_waitCounter >= _waitTime)
            _waiting = false;
    }
}