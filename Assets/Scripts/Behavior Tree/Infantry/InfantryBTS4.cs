using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using System;

public class InfantryBTS4 : Tree
{
    [UnityEngine.SerializeField] private UnityEngine.Transform leftPatrolPoint;
    [UnityEngine.SerializeField] private UnityEngine.Transform rightPatrolPoint;
    [UnityEngine.SerializeField] private UnityEngine.Transform targetTransform;
    [UnityEngine.SerializeField] private float patrolSpeed;
    [UnityEngine.SerializeField] private float waitTime;
    [UnityEngine.SerializeField] private UnityEngine.LayerMask groundLayer;
    [UnityEngine.SerializeField] private string enemyTag;
    [UnityEngine.SerializeField] private UnityEngine.Transform _enemyTarget;
    [UnityEngine.SerializeField] private float spotRange;
    [UnityEngine.SerializeField] private float attackRange;
    [UnityEngine.SerializeField] private float wanderSpeed;
    [UnityEngine.SerializeField] private float chaseSpeed;
    [UnityEngine.SerializeField] private float damageAmount;
    [UnityEngine.SerializeField] protected Health _enemyHealth;

    [UnityEngine.SerializeField] private UnityEngine.GameObject _foundEnemy;

    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new CheckSoldierInFOV(this.transform, attackRange, enemyTag),
                new TaskAttack(this.transform, ref _enemyHealth, damageAmount)
            }),

            new Sequence(new List<Node>
            {
                new CheckSoldierInFOV(this.transform, spotRange, enemyTag),
                new TaskGoToTarget(this.transform, _enemyTarget, chaseSpeed)
            }),

            new TaskPatrol(this.transform, leftPatrolPoint, rightPatrolPoint, patrolSpeed, waitTime)

        });
        return root;
    }

    private void OnDrawGizmos()
    {
        if(!this.enabled)
            return;
        UnityEngine.Gizmos.color = UnityEngine.Color.yellow;
        UnityEngine.Gizmos.DrawWireSphere(this.transform.position, spotRange);
        UnityEngine.Gizmos.DrawLine(this.transform.position, targetTransform.position);
        UnityEngine.Gizmos.color = UnityEngine.Color.red;
        UnityEngine.Gizmos.DrawWireSphere(this.transform.position, attackRange);
    }

   private void OnEnable()
    {
        CheckSoldierInFOV.OnSoldierFound += (UnityEngine.GameObject target, Health health) => UpdateTarget(target.transform);
        CheckSoldierInFOV.OnSoldierFound += (UnityEngine.GameObject target, Health health) => UpdateFoundEnemy(target, health);
        leftPatrolPoint = UnityEngine.GameObject.Find("LeftCastlePointClose").transform;
        rightPatrolPoint = UnityEngine.GameObject.Find("RightCastlePointClose").transform;
    }

    private void OnDisable()
    {
        CheckSoldierInFOV.OnSoldierFound -= (UnityEngine.GameObject target, Health health) => UpdateTarget(target.transform);
        CheckSoldierInFOV.OnSoldierFound -= (UnityEngine.GameObject target, Health health) => UpdateFoundEnemy(target, health);
    }

    private void UpdateTarget(UnityEngine.Transform enemyTarget)
    {
        _enemyTarget = enemyTarget;
    }

    private void UpdateFoundEnemy(UnityEngine.GameObject foundEnemyTarget, Health enemyHealth)
    {
        _foundEnemy = foundEnemyTarget;
        _enemyHealth = enemyHealth;
    }
}