using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraReferences : MonoBehaviour
{
    //This script is placed on Cinemachine

    [SerializeField] private CinemachineVirtualCamera virtualCam;
    [SerializeField] private GameObject CamSwitch;
    PhotonView myPv;

    private void OnEnable()
    {
        // virtualCam = this.GetComponent<CinemachineVirtualCamera>();
        CamSwitch.transform.SetParent(null);
        myPv = GetComponent<PhotonView>();
        if (!myPv.IsMine)
        {
            gameObject.SetActive(false);
        }
        if (myPv.IsMine)
        {

            virtualCam.Follow = PlayerController.instance.gameObject.transform;
            virtualCam.LookAt = PlayerController.instance.gameObject.transform;

        }
    }
}
