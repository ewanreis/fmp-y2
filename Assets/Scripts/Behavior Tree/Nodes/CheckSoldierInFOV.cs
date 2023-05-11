using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckSoldierInFOV : Node
{
    public static event System.Action<GameObject, Health> OnSoldierFound;
    private Transform _transform;
    //private LayerMask _checkMask;
    private Collider2D[] colliders;
    private float _visionRange;
    private string _checkTag;
    private List<GameObject> foundEnemies;

    public CheckSoldierInFOV(Transform transform, float visionRange, string checkTag)
    {
        _transform = transform;
        _visionRange = visionRange;
        _checkTag = checkTag;
    }

    public override NodeState Evaluate()
    {
        object t = null;
        if(t == null)
        {
            ScanSurroundings();

            if(foundEnemies.Count > 0)
            {
                parent.parent.SetData("target", colliders[0].transform);
                parent.parent.SetData("targetHealth", foundEnemies[0].GetComponent<Health>());
                //Debug.Log($"Found Target {_visionRange}");
                OnSoldierFound?.Invoke(foundEnemies[0], foundEnemies[0].GetComponent<Health>());
                state = NodeState.Success;
                return state;
            }
            state = NodeState.Failure;
            return state;
        }
        state = NodeState.Success;
        return state;
    }

    private void ScanSurroundings()
    {
        foundEnemies = new List<GameObject>();

        colliders = Physics2D.OverlapCircleAll(_transform.position, _visionRange);
        //Debug.Log(colliders.Length);

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.CompareTag(_checkTag) && Vector3.Distance(_transform.position, collider.transform.position) <= _visionRange)
            {
                foundEnemies.Add(collider.gameObject);
            }
        }
    }
}
