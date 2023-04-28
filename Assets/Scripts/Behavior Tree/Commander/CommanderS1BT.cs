using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using System;

public class CommanderS1BT : Tree
{
    [UnityEngine.SerializeField] private UnityEngine.LayerMask groundLayer;
    [UnityEngine.SerializeField] private string enemyTag;
    [UnityEngine.SerializeField] private UnityEngine.Transform targetTransform;
    [UnityEngine.SerializeField] private UnityEngine.Transform _enemyTarget;
    [UnityEngine.SerializeField] private float spotRange;
    [UnityEngine.SerializeField] private float attackRange;
    [UnityEngine.SerializeField] private float damageAmount;
    [UnityEngine.SerializeField] private float wanderSpeed;
    [UnityEngine.SerializeField] private float chaseSpeed;

    [UnityEngine.SerializeField] private UnityEngine.GameObject _foundEnemy;

    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new CheckSoldierInFOV(this.transform, spotRange, enemyTag),
                new TaskGoToTarget(this.transform, ref _enemyTarget, chaseSpeed),
                new CheckSoldierInFOV(this.transform, attackRange, enemyTag),
                new TaskAttack(this.transform, _foundEnemy, damageAmount)
            }),

            new TaskWanderEnemy(this.transform, groundLayer, wanderSpeed, targetTransform, EnemyTypes.Commander)

        });
        return root;
    }

    private void OnEnable()
    {
        CheckSoldierInFOV.OnSoldierFound += (UnityEngine.Transform target) => UpdateTarget(ref target);
        CheckSoldierInFOV.OnSoldierFound += (UnityEngine.Transform target) => UpdateFoundEnemy(target.gameObject);
    }

    private void OnDisable()
    {
        CheckSoldierInFOV.OnSoldierFound -= (UnityEngine.Transform target) => UpdateTarget(ref target);
        CheckSoldierInFOV.OnSoldierFound -= (UnityEngine.Transform target) => UpdateFoundEnemy(target.gameObject);
    }

    private void UpdateTarget(ref UnityEngine.Transform enemyTarget)
    {
        _enemyTarget = enemyTarget;
    }

    private void UpdateFoundEnemy(UnityEngine.GameObject foundEnemyTarget)
    {
        _foundEnemy = foundEnemyTarget;
    }
}
