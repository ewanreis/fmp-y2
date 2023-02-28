using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> cameraList;

    void Start()
    {
        MountableObject.OnMount += LookAtThrone;
        MountableObject.OnUnmount += LookAtPlayer;

        LookAtPlayer();
    }

    private void LookAtPlayer()
    {
        foreach(GameObject obj in cameraList)
        {
            obj.SetActive(false);
        }
        cameraList[0].SetActive(true);
        cameraList[2].SetActive(true);

    }

    private void LookAtThrone()
    {
        foreach(GameObject obj in cameraList)
        {
            obj.SetActive(false);
        }
        cameraList[1].SetActive(true);
        cameraList[3].SetActive(true);
    }
}


