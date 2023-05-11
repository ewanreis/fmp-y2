using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class TaskAttack : Node
{
    private GameObject _target;
    private Transform _transform;
    private float _attackTime = 1f;
    private float _attackCounter = 0f;
    private Health _enemyHealth;
    private float _damageAmount;

    public TaskAttack(Transform transform, ref Health enemyHealth, float damageAmount)
    {
        _enemyHealth = enemyHealth;
        _transform = transform;
        _damageAmount = damageAmount;
    }

    public override NodeState Evaluate()
    {
        _attackCounter += Time.deltaTime;
        if(_attackCounter >= _attackTime)
        {
            _enemyHealth = (Health) parent.parent.GetData("targetHealth");
            
            if (_enemyHealth != null)
                _enemyHealth.Damage(_damageAmount);
            _attackCounter = 0f;
        }

        state = NodeState.Running;
        return state;
    }
}
