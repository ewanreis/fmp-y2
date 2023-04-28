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
    private Health enemyHealth;
    private float _damageAmount;

    public TaskAttack(Transform transform, GameObject foundEnemy, float damageAmount)
    {
        _target = foundEnemy;
        _transform = transform;
        _damageAmount = damageAmount;
    }

    public override NodeState Evaluate()
    {
        _attackCounter += Time.deltaTime;
        if(_attackCounter >= _attackTime)
        {
            Debug.Log(_target.gameObject);
            enemyHealth = _target.gameObject.GetComponent<Health>();
            enemyHealth.Damage(_damageAmount);
            Debug.Log($"Attack {enemyHealth}");
            _attackCounter = 0f;
        }

        state = NodeState.Running;
        return state;
    }
}
