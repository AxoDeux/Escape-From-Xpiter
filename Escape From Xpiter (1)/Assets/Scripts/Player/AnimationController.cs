using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private AudioSource joggingAudio = null;
    [SerializeField] private AudioSource jumpStartAudio = null;
    [SerializeField] private AudioSource jumpEndAudio = null;

    [SerializeField] private AudioSource breakingAudio = null;
    [SerializeField] private AudioSource spaceBoxInteractAudio = null;


    private Animator _animator;

    private PlayerInput _playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction breakAction;
    private InputAction interactAction;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();

        moveAction = _playerInput.actions["Move"];
        jumpAction = _playerInput.actions["Jump"];
        breakAction = _playerInput.actions["Break"];
        interactAction = _playerInput.actions["Interact"];
    }

    private void OnEnable()
    {
        moveAction.performed += OnJogging;
        moveAction.canceled += StopJogging;

        jumpAction.started += OnJump;
        jumpAction.canceled += JumpEnd;

        interactAction.performed += OnInteract;
        interactAction.canceled += InteractEnd;

        breakAction.started += OnBreaking;
        breakAction.canceled += BreakingEnd;
    }
    private void OnDisable()
    {
        moveAction.performed -= OnJogging;
        moveAction.canceled -= StopJogging;

        jumpAction.started -= OnJump;
        jumpAction.canceled -= JumpEnd;

        interactAction.performed -= OnInteract;
        interactAction.canceled -= InteractEnd;

        breakAction.started -= OnBreaking;
        breakAction.canceled -= BreakingEnd;
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnJogging(InputAction.CallbackContext context)
    {
        _animator.SetBool("isJogging", true);
        joggingAudio.Play();
    }
    private void StopJogging(InputAction.CallbackContext context)
    {
        _animator.SetBool("isJogging", false);
        joggingAudio.Stop();
    }
    private void OnJump(InputAction.CallbackContext context)
    {
        _animator.SetBool("isJumping", true);
        jumpStartAudio.Play();
        joggingAudio.Stop();
    }

    private void JumpEnd(InputAction.CallbackContext context)
    {
         _animator.SetBool("isJumping", false);
        jumpEndAudio.Play();

        if (_animator.GetBool("isJogging"))
        {
            joggingAudio.Play();
        }

    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        _animator.SetBool("isInteracting", true);
    }

    private void InteractEnd(InputAction.CallbackContext context)
    {
        _animator.SetBool("isInteracting", false);
    }

    private void OnBreaking(InputAction.CallbackContext context)
    {
        breakingAudio.Play();
        _animator.SetBool("isBreaking", true);
    }

    private void BreakingEnd(InputAction.CallbackContext context)
    {
        breakingAudio.Stop();
        _animator.SetBool("isBreaking", false);
    }
}
