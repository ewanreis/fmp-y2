using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainedObject : MonoBehaviour
{
    public int length; // # of segments in the chain
    public LineRenderer lineRend;
    public Vector3[] segmentPoses; // positions of the segments in the chain
    public Transform targetDir; // direction the chain points towards
    public float targetDist; // distance between each segment
    public float smoothSpeed;
    public float trailSpeed;
    private Vector3 segmentV; // current velocity of the segment movement

    public float wiggleSpeed;
    public float wiggleMagnitude;
    public Transform wiggleDir;
    public Transform[] bodyParts; // the parts of the chain

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
