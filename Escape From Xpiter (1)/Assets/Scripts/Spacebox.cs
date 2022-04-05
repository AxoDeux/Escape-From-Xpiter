using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Photon.Pun;
using UnityEngine.UI;

public class Spacebox : MonoBehaviour
{
    [SerializeField] private GameObject miniGameCanvas;
    [SerializeField] private GameObject spaceboxMesh = null;
    [SerializeField] private AudioSource correctAudio = null;
    [SerializeField] private AudioSource wrongAudio = null;
    [SerializeField] private AudioSource spaceboxOpen = null;


    private int spaceboxNo = 1;


    public static event Action SpaceBoxInteracted;
    public static event Action<bool> GiveReward;    //Called on grid

    public PlayerInput playerInput;
    private InputAction interactAction;

    private bool isPlayerNearby = false;
    private bool isSpaceboxSolved = false;
    public static bool isQuestionCanvasActive = false;

    private List<PlayerController> playersInRange = new List<PlayerController>();

    private PhotonView myPhotonView;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        interactAction = playerInput.actions["Interact"];
    }

    private void OnEnable()
    {
        interactAction.performed += PlayerInteract;
    }

    private void Start()
    {

        myPhotonView = PhotonView.Get(this);
    }

    private void OnDisable()
    {
        interactAction.performed -= PlayerInteract;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) { return; }
        isPlayerNearby = true;
        //    playersInRange.Add(other.gameObject.GetComponent<PlayerController>());
        Debug.Log(spaceboxNo);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) { return; }
        //   playersInRange.Remove(other.gameObject.GetComponent<PlayerController>());

        // if (playersInRange.Count == 0)
        //{
        //     isPlayerNearby = false;
        //}
        isPlayerNearby = false;
    }

    private void PlayerInteract(InputAction.CallbackContext context)
    {
        // if (!myPhotonView.IsMine) { return; } because this object is not instantiated & thus belongs to master client only

        if (!isPlayerNearby) { return; }
        if (isSpaceboxSolved) { return; }
        if (isQuestionCanvasActive) { return; }

        Debug.Log("interact button pressed");

        //Write code to separate out the player who pressed the E button and the player who is just in range of the spacebox
        //Also need to check that the person in range is the one who pressed the interact key
        if (!PlayerController.instance.myPv.IsMine) { return; }

        // Debug.Log("E pressed and player within range!");

        isQuestionCanvasActive = true;
        myPhotonView.RPC("RPC_InvokeInteractionEvent", RpcTarget.All, isQuestionCanvasActive);
        PlayerController.instance.myPv.RPC("HandleTotalMoveCount", RpcTarget.All, "SpaceboxOpened");
        miniGameCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        myPhotonView.RPC("QuestionScreen", RpcTarget.Others, isQuestionCanvasActive);

    }

    public void OnMiniGameClosed(bool isSolved)
    {
        isQuestionCanvasActive = false;

        myPhotonView.RPC("RPC_InvokeInteractionEvent", RpcTarget.All, isQuestionCanvasActive);

        myPhotonView.RPC("QuestionScreen", RpcTarget.All, isQuestionCanvasActive);




        //  isSpaceboxSolved = isSolved;
        //Check solution
        if (isSolved)
        {
            //this.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            // spaceBoxColor = new Vector3() { Color.green.r, Color.green.g, Color.green.b };
            myPhotonView.RPC("SetColor", RpcTarget.All, isSolved);
            myPhotonView.RPC(nameof(HandleGiveReward), RpcTarget.All, isSolved);
        }
        else
        {
            // this.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            //  spaceBoxColor = Color.red;
            myPhotonView.RPC("SetColor", RpcTarget.All, isSolved);
            //StartCoroutine(AnsweredWrong());
        }
    }

    /* private IEnumerator AnsweredWrong()
     {
         yield return new WaitForSeconds(4f);
         this.GetComponent<Renderer>().material.SetColor("_Color", spaceBoxColor);
     }*/

    [PunRPC]
    private void HandleGiveReward(bool isSolved)        //to show reward on all clients
    {
        GiveReward.Invoke(isSolved);
    }
    [PunRPC]
    void SetColor(bool isSolved)
    {
        isSpaceboxSolved = isSolved;
        if (isSolved)
        {
            spaceboxMesh.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            correctAudio.Play();
        }
        else
        {
            spaceboxMesh.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            wrongAudio.Play();
        }

    }
    [PunRPC]
    void QuestionScreen(bool questionVisible)
    {

        if (!questionVisible)
        {
            miniGameCanvas.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            miniGameCanvas.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
        }


    }
    [PunRPC]
    void RPC_InvokeInteractionEvent(bool isQuestionVisible)
    {
        SpaceBoxInteracted.Invoke();
        spaceboxOpen.Play();
        isQuestionCanvasActive = isQuestionVisible;
    }
}

