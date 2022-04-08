using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(PlayerInput), typeof(BoxCollider))]
public class Wall : MonoBehaviour
{
    [SerializeField] private Image countDownImage;
    [SerializeField] private Image countDownImage2;

    [SerializeField] private TMP_Text intructionText = null;

    private PlayerInput playerInput;
    private InputAction breakWallsAction;
    bool isPlayerNearby = false;
    bool isBreaking = false;

    private float wallTimer = 0f;
    private float wallBreakDuration = 3f;
    private float progressImageVelocity;
    private PhotonView myWallPV;
    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        breakWallsAction = playerInput.actions["Break"];
    }
    void Start()
    {
        myWallPV = GetComponent<PhotonView>();
    }
    private void OnEnable()
    {
        breakWallsAction.performed += BeginWallBreak;
        breakWallsAction.canceled += StopBreakWall;
    }

    private void OnDisable()
    {
        breakWallsAction.performed -= BeginWallBreak;
        breakWallsAction.canceled -= StopBreakWall;
    }

    private void Update()
    {
        if (isBreaking)
        {
            wallTimer += Time.deltaTime;
            HandleBreakUIDisplay();
            if (wallTimer < wallBreakDuration) { return; }          //return is the timer isnt over yet

            wallTimer = 0f;
            isBreaking = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerNearby = true;
        intructionText.text = "Break";
        intructionText.gameObject.SetActive(true);
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerNearby = false;
        intructionText.gameObject.SetActive(false);
    }

    private void BeginWallBreak(InputAction.CallbackContext context)            // call when player begins breaking
    {
        //  if (!myWallPV.IsMine) { return; } ( because this object is not instantiated )
        Debug.Log("Interaction performed!");
        if (!isPlayerNearby) { return; }

        countDownImage.gameObject.SetActive(true);
        countDownImage2.gameObject.SetActive(true);

        isBreaking = true;
        Debug.Log("Destroy wall function called!");
        StartCoroutine(StartWallBreaking());
    }

    private IEnumerator StartWallBreaking()                                //Delay in breaking the wall
    {
        yield return new WaitForSeconds(wallBreakDuration);
        //gameObject.SetActive(false);
        myWallPV.RPC("DestroyWall", RpcTarget.All);

        PlayerController.instance.myPv.RPC("HandleTotalMoveCount", RpcTarget.All, "BreakWall");
    }

    private void StopBreakWall(InputAction.CallbackContext context)     //Call when player stops breaking
    {
        StopAllCoroutines();
        countDownImage.gameObject.SetActive(false);
        countDownImage2.gameObject.SetActive(false);

        isBreaking = false;
        wallTimer = 0f;
    }

    private void HandleBreakUIDisplay()                             //Handles the countdown image progress
    {
        float newProgress = wallTimer / (wallBreakDuration - 0.2f);
        if (newProgress < countDownImage.fillAmount)
        {
            countDownImage.fillAmount = newProgress;
        }
        else
        {
            countDownImage.fillAmount = Mathf.SmoothDamp(countDownImage.fillAmount, newProgress, ref progressImageVelocity, 0.1f);
        }
        if (newProgress < countDownImage2.fillAmount)
        {
            countDownImage2.fillAmount = newProgress;
        }
        else
        {
            countDownImage2.fillAmount = Mathf.SmoothDamp(countDownImage2.fillAmount, newProgress, ref progressImageVelocity, 0.1f);
        }
    }
    [PunRPC]
    void DestroyWall()
    {
        Destroy(gameObject);

        //gameObject.SetActive(false);
        Debug.Log("Wall destroyed!");
    }

    /*void BreakWall(InputAction.CallbackContext context)
    {
    if (!isPlayerNearby)
        return;
    Debug.Log("Break the wall");
    gameObject.SetActive(false);
    PlayerController.instance.HandleTotalMoveCount("BreakWall");
    }*/
}
