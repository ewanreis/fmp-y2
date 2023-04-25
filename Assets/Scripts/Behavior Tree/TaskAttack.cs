using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class TaskAttack : Node
{
    private Transform _target;
    private float _attackTime = 1f;
    private float _attackCounter = 0f;

    public TaskAttack(Transform target)
    {
        _target = target;
    }

    public override NodeState Evaluate()
    {
        //Transform target = (Transform)GetData("target");

        /*if(target != _lastTarget)
        {
            // TODO Get Enemy Manager
            _lastTarget = target;
        }*/


        _attackCounter += Time.deltaTime;
        if(_attackCounter >= _attackTime)
        {
            // TODO Attack
            _attackCounter = 0f;
        }

        state = NodeState.Running;
        return state;
    }
}
