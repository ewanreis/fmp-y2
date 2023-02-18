using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainedObject : MonoBehaviour
{
    public int length;
    public LineRenderer lineRend;
    public Vector3[] segmentPoses;
    public Transform targetDir;
    public float targetDist;
    public float smoothSpeed;
    public float trailSpeed;
    private Vector3 segmentV;

    public float wiggleSpeed;
    public float wiggleMagnitude;
    public Transform wiggleDir;
    public Transform[] bodyParts;

    // Start is called before the first frame update
    void Start()
    {
        lineRend.positionCount = length;
        segmentPoses = new Vector3[length];
        segmentPoses = new Vector3[length];
    }

    // Update is called once per frame
    void Update()
    {
        wiggleDir.localRotation = Quaternion.Euler(0,0, Mathf.Sin(Time.time * wiggleSpeed) * wiggleMagnitude);

        segmentPoses[0] = targetDir.position;
        for (int i = 1; i < segmentPoses.Length; i++)
        {
            /*segmentPoses[i] = Vector3.SmoothDamp
            (
                segmentPoses[i], 
                segmentPoses[i - 1] + targetDir.right * targetDist, 
                ref segmentV,
                smoothSpeed + i / trailSpeed);*/

            Vector3 targetPos = segmentPoses[i - 1] + (segmentPoses[i] - segmentPoses[i - 1]).normalized * targetDist;
            segmentPoses[i] = Vector3.SmoothDamp
            (
                segmentPoses[i], 
                targetPos, 
                ref segmentV, 
                smoothSpeed
            );
            bodyParts[i - 1].transform.position = segmentPoses[i];
        }
        lineRend.SetPositions(segmentPoses);
    }
}
