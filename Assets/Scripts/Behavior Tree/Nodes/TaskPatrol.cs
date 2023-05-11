using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
public class TaskPatrol : Node
{
    public static event System.Action OnReachPoint;
    private Transform _transform;
    private Transform leftPoint;
    private Transform rightPoint;
    private Transform target;
    private float _walkSpeed;
    private float _waitCounter;
    private float _waitTime;
    private bool _waiting;
    public TaskPatrol(Transform transform, Transform leftPatrolPoint, Transform rightPatrolPoint, float walkSpeed, float waitTime)
    {
        leftPoint = leftPatrolPoint;
        rightPoint = rightPatrolPoint;
        _walkSpeed = walkSpeed;
        _transform = transform;
        target = leftPatrolPoint;
        _waitTime = waitTime;
        //Debug.Log($"{transform},{leftPatrolPoint},{rightPatrolPoint},{walkSpeed},{waitTime}");
    }

    public override NodeState Evaluate()
    {
        if(_waiting)
            CheckWait();

        else
        {
            if(Vector3.Distance(_transform.position, target.position) < 1f)
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
        
        if(target == leftPoint)
            target = rightPoint;
        else
            target = leftPoint;
    }

    private void MoveToTargetPoint()
    {
        _transform.position = Vector3.MoveTowards(_transform.position, target.position, _walkSpeed * Time.deltaTime);
        Debug.DrawLine(_transform.position, target.position, Color.green);
    }

    private void CheckWait()
    {
        _waitCounter += Time.deltaTime;

        if(_waitCounter >= _waitTime)
            _waiting = false;
    }
}
