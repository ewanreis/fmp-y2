using System.Drawing;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using System;

public class CommanderBTS3 : Tree
{
    [UnityEngine.SerializeField] private UnityEngine.LayerMask groundLayer;
    [UnityEngine.SerializeField] private string enemyTag;
    [UnityEngine.SerializeField] private UnityEngine.Transform targetTransform;
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
                new CheckSoldierInFOV(this.transform, attackRange - 0.3f, enemyTag),
                new TaskAttack(this.transform, ref _enemyHealth, damageAmount)
            }),

            new Sequence(new List<Node>
            {
                new CheckSoldierInFOV(this.transform, spotRange, enemyTag),
                new TaskGoToTarget(this.transform, _enemyTarget, chaseSpeed)
            }),

            new TaskWander(this.transform, groundLayer, wanderSpeed, targetTransform)

        });
        return root;
    }

    private void OnDrawGizmos()
    {
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
