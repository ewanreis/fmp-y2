using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainedObject : MonoBehaviour
{
    //* This code will chain objects in a line for use in things such as chains, vines or centipedes

    #region References
    [SerializeField] [Tooltip("Number of segments in the chain")] 
    private int length;

    [SerializeField] 
    private LineRenderer lineRend;

    [SerializeField] [Tooltip("Positions of the segments in the chain")] 
    private Vector3[] segmentPoses;

    [SerializeField] [Tooltip("Direction the chain points towards")] 
    private Transform targetDir;

    [SerializeField] [Tooltip("Distance between each segment")] 
    private float targetDist;

    [SerializeField] 
    private float smoothSpeed;

    [SerializeField] 
    private float trailSpeed;

    [SerializeField] 
    private float wiggleSpeed;
    [SerializeField] 
    private float wiggleMagnitude;

    [SerializeField] 
    private Transform wiggleDir;

    [SerializeField] [Tooltip("The parts of the chain")] 
    private Transform[] bodyParts;
    #endregion

    private Vector3 segmentV; // current velocity of the segment movement

    void Start()
    {
        lineRend.positionCount = length;
        segmentPoses = new Vector3[length];
        segmentPoses = new Vector3[length];
    }

    void Update()
    {
        // apply a sine wave wiggle effect
        wiggleDir.localRotation = Quaternion.Euler(0,0, Mathf.Sin(Time.time * wiggleSpeed) * wiggleMagnitude);

        // move segments towards the target direction
        segmentPoses[0] = targetDir.position;
        for (int i = 1; i < segmentPoses.Length; i++)
        {
            // calculate target position of segment based on previous segment position and the target distance
            Vector3 targetPos = segmentPoses[i - 1] + (segmentPoses[i] - segmentPoses[i - 1]).normalized * targetDist;

            // smooth position
            segmentPoses[i] = Vector3.SmoothDamp
            (
                segmentPoses[i], 
                targetPos, 
                ref segmentV, 
                smoothSpeed
            );

            // set transform of parts
            bodyParts[i - 1].transform.position = segmentPoses[i];
        }

        lineRend.SetPositions(segmentPoses);
    }
}
