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
    private bool isOnCheck = false;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        interactAction = playerInput.actions["interact"];
    }

    private void OnEnable()
    {
        interactAction.performed += HandleStartInteraction;
    }

    private void OnDisable()
    {
        interactAction.performed -= HandleStartInteraction;
    }

    private void HandleStartInteraction(InputAction.CallbackContext obj)
    {
        if (!inRange) { return; }
        if (isOnCheck) { return; }
        Debug.Log("E pressed");
        isOnCheck = true;
        assemblyCenter.IncreaseCount(gameObject.transform.GetChild(0).gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) { return; }
        inRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) { return; }
        inRange = false;
    }
}
