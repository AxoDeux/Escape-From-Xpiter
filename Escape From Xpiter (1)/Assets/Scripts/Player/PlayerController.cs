using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]        //this script requires these components along with it
public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private AnimationController playerAnimationController;


    //[SerializeField] private Slider oxygenLevel;
    public float oxygenLevel;

    private CharacterController controller;
    public PlayerInput playerInput;
    private Transform cameraTransform;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private string spaceboxTag = "SpaceBox";
    private bool onPlayerActionMap = true;

    private InputAction moveAction;
    private InputAction jumpAction;

    // AstronautCustomisation
    static Color myPlayerColor;
    [SerializeField] GameObject playerMat;
    [SerializeField] Image canvasImage;
    static string playerName;


    // Moves!
    public static int totalMoves;
    bool isCalled = false;

    // Multiplayer
    public PhotonView myPv;
    public Transform myPlayerTransform;

    // SpaceShip Jigsaw

    private InputAction interactAction;
    private bool isSpaceShipNearby = false;
    public static event Action _SpaceshipJigsaw;
    private void Awake()
    {
        myPv = GetComponent<PhotonView>();
        /*if (myPlayerColor == Color.clear)
        {
            myPlayerColor = Color.white;
        }*/
        //cacheing components and actions
        if (myPv.IsMine)
        {
            instance = this;
        }
        myPlayerTransform = GetComponent<Transform>();
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        interactAction = playerInput.actions["Interact"];

        controller.detectCollisions = false;        //character controller does act as a collider now
                                                    //  Cursor.lockState = CursorLockMode.Locked;

    }

    private void Start()
    {

        totalMoves = 0;
        playerVelocity.y = 0f;
        cameraTransform = Camera.main.transform;

        if (!myPv.IsMine) //remove rb of other players from my game instance                  
        {
            Destroy(GetComponent<Rigidbody>());
            playerAnimationController.enabled = false;

        }
        if (myPv.IsMine)
        {
            myPv.RPC(nameof(RPC_SetPlayerColor), RpcTarget.AllBuffered,
                   new Vector3(myPlayerColor.r, myPlayerColor.g, myPlayerColor.b));
        }
    }

    private void OnEnable()
    {
        interactAction.performed += PlayerInteract;
        Spacebox.SpaceBoxInteracted += HandleInputChange;
        UI_Manager.OptionsUpdated += HandleOptionsChange;
        _SpaceshipJigsaw += HandleInputChange;
    }

    private void OnDisable()
    {
        interactAction.performed -= PlayerInteract;
        Spacebox.SpaceBoxInteracted -= HandleInputChange;
        _SpaceshipJigsaw -= HandleInputChange;
        UI_Manager.OptionsUpdated += HandleOptionsChange;

    }


    void Update()
    {

        if (!myPv.IsMine) { return; }
        if (oxygenLevel > 0f) { oxygenLevel -= Time.deltaTime; }
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 input = moveAction.ReadValue<Vector2>();

        Vector3 move = new Vector3(input.x, 0, input.y);

        //multiple move axis input with camera direction so that the player moves in the direction the camera is facing
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        move.y = 0f;

        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Changes the height position of the player..
        if (jumpAction.triggered && groundedPlayer)
        {
            //playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);            //Commented because animation has change in height
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        //rotating player where the camera is facing
        Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public void HandleInputChange()
    {
        if (onPlayerActionMap)
        {
            playerInput.SwitchCurrentActionMap("UI");
            onPlayerActionMap = false;

            Debug.Log("ActionMapChanged to " + playerInput.currentActionMap.ToString());
        }
        else
        {
            playerInput.SwitchCurrentActionMap("Player");
            onPlayerActionMap = true;

            Debug.Log("ActionMapChanged to " + playerInput.currentActionMap.ToString());
        }

    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Tile"))
        {
            if (myPv.IsMine)
            {
                myPv.RPC("HandleTotalMoveCount", RpcTarget.All, "PlayerMoved");
                //Debug.Log("TriggerEntered");
            }
            //Debug.Log(other.name);
        }
        if (other.CompareTag("SpaceShipArea"))
        {

            if (AssemblyCenter.totalButtonsPressed == 2)
                isSpaceShipNearby = true;

        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SpaceShipArea"))
        {
            isSpaceShipNearby = false;

        }
    }

    private void PlayerInteract(InputAction.CallbackContext context)
    {
        if (!myPv.IsMine) { return; }
        if (!isSpaceShipNearby) { return; }
        Cursor.lockState = CursorLockMode.Confined;
        _SpaceshipJigsaw.Invoke();
        myPv.RPC(nameof(InvokeJigsawCanvas), RpcTarget.Others);

    }
    private void HandleOptionsChange(int energy)
    {
        totalMoves = energy;
    }

    public void SetAstronautColor(Color color)
    {
        Debug.Log("myPlayerColor changed");
        myPlayerColor = color;
        Debug.Log(myPlayerColor);

    }
    public void SetAstronautName(string name)
    {
        playerName = name;
        Debug.Log(playerName);
    }

    [PunRPC]
    void InvokeJigsawCanvas()
    {
        _SpaceshipJigsaw.Invoke();

    }
    [PunRPC]
    public void HandleTotalMoveCount(string eventType)
    {
        // Call this code in a RPC method
        if (eventType == "BreakWall")
        {
            totalMoves += 3;
            Debug.Log(PlayerController.totalMoves);
        }
        else if (eventType == "MapViewChanged")
        {
            totalMoves += 5;
        }
        else if (eventType == "SpaceboxOpened")
        {
            totalMoves += 3;
        }
        else
        {
            totalMoves += 1;
        }
        //instance.moveCountText.text = "Energy Cells:" + totalMoves.ToString();
    }
    [PunRPC]
    public void RPC_SetPlayerColor(Vector3 playerColor)
    {
        Color color = new Color(playerColor.x, playerColor.y, playerColor.z);
        if (playerMat.GetComponent<Renderer>() != null)
        {
            Debug.Log("player mat color changed");

            playerMat.GetComponent<Renderer>().material.SetColor("_Color", color);
        }
        if (canvasImage != null)
        {
            canvasImage.color = color;
            Debug.Log("Player image color changed!");
        }
    }

}