using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    //* Manages foreground and background cameras switching when mounting and dismounting
    [SerializeField] private List<GameObject> cameraList;

    void OnEnable()
    {
        MountableObject.OnMount += LookAtThrone;
        MountableObject.OnUnmount += LookAtPlayer;

        LookAtPlayer();
    }

    void OnDisable()
    {
        MountableObject.OnMount -= LookAtThrone;
        MountableObject.OnUnmount -= LookAtPlayer;
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


