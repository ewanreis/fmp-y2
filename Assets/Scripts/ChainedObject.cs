using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainedObject : MonoBehaviour
{
    [SerializeField] private int length; // # of segments in the chain
    [SerializeField] private LineRenderer lineRend;
    [SerializeField] private Vector3[] segmentPoses; // positions of the segments in the chain
    [SerializeField] private Transform targetDir; // direction the chain points towards
    [SerializeField] private float targetDist; // distance between each segment
    [SerializeField] private float smoothSpeed;
    [SerializeField] private float trailSpeed;

    [SerializeField] private float wiggleSpeed;
    [SerializeField] private float wiggleMagnitude;
    [SerializeField] private Transform wiggleDir;
    [SerializeField] private Transform[] bodyParts; // the parts of the chain

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
