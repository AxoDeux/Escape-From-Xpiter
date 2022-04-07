using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class ButtonPress : MonoBehaviour
{
    [SerializeField] private AssemblyCenter assemblyCenter = null;

    private PlayerInput playerInput;
    private InputAction interactAction;

    private bool inRange = false;
    private bool pressedCheck = false;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        interactAction = playerInput.actions["interact"];
    }

    private void OnEnable()
    {
        interactAction.started += HandleStartInteraction;
        interactAction.canceled += HandleCancelInteraction;
    }

    private void OnDisable()
    {
        interactAction.started -= HandleStartInteraction;
        interactAction.canceled -= HandleCancelInteraction;
    }

    private void HandleStartInteraction(InputAction.CallbackContext obj)
    {
        if (!inRange) { return; }
        Debug.Log("E pressed");
        assemblyCenter.IncreaseCount();        
    }

    private void HandleCancelInteraction(InputAction.CallbackContext context)
    {
        if (!inRange) { return; }
        Debug.Log("E Cancelled");
        assemblyCenter.DecreaseCount();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) { return; }
        inRange = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) { return; }
        inRange = false;
    }
}
