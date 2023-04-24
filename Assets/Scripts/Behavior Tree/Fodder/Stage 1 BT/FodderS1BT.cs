using System.Collections;
using System.Collections.Generic;
using BehaviorTree;

public class FodderS1BT : Tree
{
    [UnityEngine.SerializeField] private UnityEngine.LayerMask groundLayer;
    [UnityEngine.SerializeField] private UnityEngine.Transform targetTransform;
    protected override Node SetupTree()
    {
        Node root = new Sequence(new List<Node>
        {
            new TaskWander(this.transform, groundLayer, 2f, targetTransform, EnemyTypes.Footsoldier)
        });
        return root;
    }
}
