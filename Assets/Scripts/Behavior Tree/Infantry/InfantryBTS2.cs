using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using System;

public class InfantryBTS2 : Tree
{
    [UnityEngine.SerializeField] private UnityEngine.Transform leftPatrolPoint;
    [UnityEngine.SerializeField] private UnityEngine.Transform rightPatrolPoint;
    [UnityEngine.SerializeField] private UnityEngine.Transform targetTransform;
    [UnityEngine.SerializeField] private float patrolSpeed;
    [UnityEngine.SerializeField] private float waitTime;


    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new TaskPatrol(this.transform, leftPatrolPoint, rightPatrolPoint, patrolSpeed, waitTime)
        });
        return root;
    }
}
