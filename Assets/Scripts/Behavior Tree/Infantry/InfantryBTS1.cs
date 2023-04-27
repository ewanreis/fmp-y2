using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using System;

public class InfantryBTS1 : Tree
{
    [UnityEngine.SerializeField] private UnityEngine.LayerMask groundLayer;
    [UnityEngine.SerializeField] private UnityEngine.Transform targetTransform;
    [UnityEngine.SerializeField] private float wanderSpeed;
    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new TaskWander(this.transform, groundLayer, wanderSpeed, targetTransform)

        });
        return root;
    }
}

