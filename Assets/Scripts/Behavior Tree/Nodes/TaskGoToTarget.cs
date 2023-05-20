using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class TaskGoToTarget : Node
{
    private Transform _transform;
    private Transform _target;
    private float _walkSpeed;

    public TaskGoToTarget(Transform transform, Transform target, float walkSpeed)
    {
        _transform = transform;
        _target = target;
        _walkSpeed = walkSpeed;
    }

    public override NodeState Evaluate()
    {
        _target = (Transform)parent.parent.GetData("target");

        if(_target == null)
            return NodeState.Failure;

        if(Vector3.Distance(_transform.position, _target.position) > 0.01f)
        {
            _transform.position = Vector3.MoveTowards
            (
                _transform.position,
                _target.position,
                _walkSpeed * Time.deltaTime
            );
        }

        state = NodeState.Running;
        return state;
    }
}
