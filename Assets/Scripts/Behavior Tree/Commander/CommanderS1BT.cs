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
    [UnityEngine.SerializeField] private float wanderSpeed;
    [UnityEngine.SerializeField] private float chaseSpeed;

    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new CheckSoldierInFOV(this.transform, spotRange, enemyTag),
                new TaskGoToTarget(this.transform, ref _enemyTarget, chaseSpeed)
            }),

            new TaskWanderEnemy(this.transform, groundLayer, wanderSpeed, targetTransform, EnemyTypes.Commander)

        });
        return root;
    }

   private void OnEnable()
    {
        CheckSoldierInFOV.OnSoldierFound += (UnityEngine.Transform target) => UpdateTarget(ref target);
    }

    private void OnDisable()
    {
        CheckSoldierInFOV.OnSoldierFound -= (UnityEngine.Transform target) => UpdateTarget(ref target);
    }

    private void UpdateTarget(ref UnityEngine.Transform enemyTarget)
    {
        //UnityEngine.Debug.Log($"{enemyTarget}");
        _enemyTarget = enemyTarget;
    }
}
