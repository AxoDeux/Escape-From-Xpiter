using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class SwitchVcam : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private int priorityBoostAmount = 10;
    [SerializeField] private Canvas NormalCanvas = null;
    [SerializeField] private Canvas AimCanvas = null;


    private CinemachineVirtualCamera virtualCamera;
    private InputAction aimAction;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        aimAction = playerInput.actions["Aim"];
    }

    private void OnEnable()
    {
        aimAction.performed += _ => StartAim();         //'_' is used to not get any parameters that come with the event
        aimAction.canceled += _ => CancelAim();         //when aim key is released
    }

    private void OnDisable()
    {
        aimAction.performed -= _ => StartAim();         //'_' is used to not get any parameters that come with the event
        aimAction.canceled -= _ => CancelAim();
    }

    private void StartAim()
    {
        virtualCamera.Priority += priorityBoostAmount;  //if  is 9 then it will become 19
        AimCanvas.enabled = true;
        NormalCanvas.enabled = false; 
    }

    private void CancelAim()
    {
        virtualCamera.Priority -= priorityBoostAmount;
        NormalCanvas.enabled = true;
        AimCanvas.enabled = false;
    }
}
