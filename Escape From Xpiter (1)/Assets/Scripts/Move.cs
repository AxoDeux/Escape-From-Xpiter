using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Order in which events are called :- pOINTERdOWN, BeginDrag, oNDRAG, Ondrop, On enddrag
public class Move : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Transform JigsawCanvas;
    [SerializeField] private Transform contentPanel;
    // [SerializeField] private GameObject repairSlider;
    //[SerializeField] private Sprite slotSprite;
    //  [SerializeField] private AudioSource OnClick = null;
    // [SerializeField] private AudioSource OnPlace = null;
    //[SerializeField] private AudioSource OnSwitch = null;


    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 halfScale = new Vector2(0.5f, 0.5f);
    private Vector2 fullScale = new Vector2(1f, 1f);
    private Vector3 prevPos;
    public bool droppedOnSlot;


    public bool isPositionCorrect = false;
    private float tweenTime = 0.5f;
    public static float timeToRepair = 0f;
    private PhotonView myPv;
    [SerializeField] private Text timeToRepairText;




    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        myPv = GetComponent<PhotonView>();
        // correctPhrasePosition = correctSlot.position;

    }
    private void Start()
    {

        prevPos = rectTransform.position;
        //   phraseSprite = GetComponent<Image>().sprite;
    }



    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("onpointerdown");

    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        // if (isPositionCorrect) { return; }
        droppedOnSlot = false;
        //  OnClick.Play();


        myPv.RPC(nameof(RPC_SetParentasJigsawCanvas), RpcTarget.All);
        //transform.SetParent(JigsawCanvas, true); // call this in rpc
        LeanTween.scale(gameObject, halfScale, tweenTime).setEaseInExpo();
        Debug.Log("Scaled Down");
        canvasGroup.alpha = 0.75f;
        canvasGroup.blocksRaycasts = false;  // if this is not done, the drop event will only be captured by image part
    }                                        // and not slot               

    public void OnDrag(PointerEventData eventData)
    {
        // if (isPositionCorrect) { return; }
        rectTransform.anchoredPosition += eventData.delta;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //if (isPositionCorrect) { return; }
        if (!droppedOnSlot)
        {
            myPv.RPC(nameof(RPC_SetParentascontentPanel), RpcTarget.All);
            //transform.SetParent(contentPanel, true); // call this in rpc
            rectTransform.position = prevPos;
            Debug.Log("Drag ended but no slot here");

        }
        //  LeanTween.scale()

        LeanTween.scale(gameObject, fullScale, tweenTime).setEaseInExpo();
        canvasGroup.alpha = 1f;
        // OnSwitch.Play();

        canvasGroup.blocksRaycasts = true;


    }

    void OnTriggerStay2D(Collider2D other)
    {

        if (gameObject.name == other.gameObject.name && droppedOnSlot && !isPositionCorrect)
        {
            //  Debug.Log(isPositionCorrect);
            isPositionCorrect = true;
            myPv.RPC(nameof(RPC_RepairPart), RpcTarget.All);


            //   Debug.Log("Correct Collider!");
        }

        //Debug.Log("WrongCollider!");
    }


    [PunRPC]
    void RPC_RepairPart()
    {
        Debug.Log("repair part function called!");
        //GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
        // GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
        isPositionCorrect = true;
        timeToRepair += 10f;
        timeToRepairText.enabled = true;

    }
    [PunRPC]
    void RPC_SetParentasJigsawCanvas()
    {
        transform.SetParent(JigsawCanvas, true);
    }
    [PunRPC]
    void RPC_SetParentascontentPanel()
    {
        transform.SetParent(contentPanel, true);
    }

    public void SetPosition(Vector3 pos)
    {
        myPv.RPC(nameof(Rpc_SycFinalPosition), RpcTarget.All, pos);
    }

    [PunRPC]
    private void Rpc_SycFinalPosition(Vector3 pos)
    {
        rectTransform.position = pos;
    }
}


