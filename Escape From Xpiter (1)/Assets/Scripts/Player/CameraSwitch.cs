using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerInput))]
public class CameraSwitch : MonoBehaviour
{
    [SerializeField] private Camera mainCam;
    [SerializeField] private Camera mapCam;
    private PhotonView myCameraPv;


    private float mapViewDuration = 10f;
    private bool isMapViewOn = false;

    [SerializeField] private GameObject mapButton = null;

    private void OnEnable()
    {
        UI_Manager.MapViewCalled += HandleCamChange;
        myCameraPv = GetComponent<PhotonView>();
    }

    private void OnDisable()
    {
        UI_Manager.MapViewCalled -= HandleCamChange;
    }

    private void Start()
    {
        mapButton = GameObject.Find("b_MapView");
        if(mapButton != null) { Debug.Log("Button Found"); }
    }

    private void HandleCamChange()
    {
        Debug.Log("MapCalled");
        if (!isMapViewOn)
        {
            if (myCameraPv.IsMine)
            {
                PlayerController.instance.myPv.RPC("HandleTotalMoveCount", RpcTarget.All, "MapViewChanged");
            }

            myCameraPv.RPC(nameof(ChangeMapScene), RpcTarget.All);

        }
    }

    [PunRPC]
    void ChangeMapScene()
    {
        /*if (!this.myCameraPv.IsMine)
            return;*/
        mapCam.gameObject.SetActive(true);
        mainCam.gameObject.SetActive(false);
        isMapViewOn = true;
        mapButton.GetComponent<Button>().interactable = false;
        StartCoroutine(MapViewDuration());
    }

    private IEnumerator MapViewDuration()
    {
        yield return new WaitForSeconds(mapViewDuration);
        mapCam.gameObject.SetActive(false);
        mainCam.gameObject.SetActive(true);
        isMapViewOn = false;
    }
}
