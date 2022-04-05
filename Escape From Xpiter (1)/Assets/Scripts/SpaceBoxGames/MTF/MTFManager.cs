using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class MTFManager : MonoBehaviour
{
    [SerializeField] private Spacebox thisSpacebox = null;
    [SerializeField] private GameObject optionsParent = null;
    [SerializeField] private Button[] chestArray = new Button[3];
    [SerializeField] private Button[] optionsArray = new Button[3];
    [SerializeField] private AudioSource buttonClick = null;

    [SerializeField] private MTFScriptableObject[] mtfQuestions;
    private MTFScriptableObject mtfValues = null;

    private Button selectedOption = null;
    private Button selectedChest = null;

    private Image chestImage = null;
    private Image chestImageSelected = null;
    private Image optionImage = null;
    private Image optionImageSelected = null;


    private TextMeshProUGUI chestA;
    private TextMeshProUGUI chestB;
    private TextMeshProUGUI chestC;
    private TextMeshProUGUI optionA;
    private TextMeshProUGUI optionB;
    private TextMeshProUGUI optionC;

    private PhotonView myPhotonView;

    private void OnEnable()
    {
        ButtonClick.OptionClicked += HandleOptionClicked;
        ButtonClick.ChestClicked += HandleChestClicked;
        thisSpacebox = transform.parent.parent.GetComponent<Spacebox>();

    }

    private void OnDisable()
    {
        ButtonClick.OptionClicked -= HandleOptionClicked;
        ButtonClick.ChestClicked -= HandleChestClicked;
    }

    private void Start()
    {
        myPhotonView = PhotonView.Get(this);

        if (PhotonNetwork.IsMasterClient)                               //Set the question once
        {
            //  myPhotonView = PhotonView.Get(this);
            int questionNo = Random.Range(0, mtfQuestions.Length);
            myPhotonView.RPC(nameof(SetQuestion), RpcTarget.All, questionNo);
        }
    }

    private void HandleOptionClicked(Button optionClicked)
    {
        buttonClick.Play();
        selectedOption = optionClicked;
    }

    private void HandleChestClicked(Button chestClicked)
    { 
        buttonClick.Play();
        selectedChest = chestClicked;

        int buttonNum = 0;
        int chestNum = 0;

        if (selectedChest.transform.childCount != 2 && selectedOption != null)
        {

            if(selectedOption == optionsArray[0])
            {
                buttonNum = 0;
            }
            if (selectedOption == optionsArray[1])
            {
                buttonNum = 1;
            }
            if (selectedOption == optionsArray[2])
            {
                buttonNum = 2;
            }

            if(selectedChest == chestArray[0])
            {
                chestNum = 0;
            }
            if (selectedChest == chestArray[1])
            {
                chestNum = 1;
            }
            if (selectedChest == chestArray[2])
            {
                chestNum = 2;
            }

            myPhotonView.RPC(nameof(ShowButtonClicked), RpcTarget.All, buttonNum, chestNum);            //Or pass string selectedOption.name and run find that button in RPC
            CheckSolution();
        }

    }

    private void CheckSolution()
    {
        for (int i = 0; i < chestArray.Length; i++)
        {
            if (chestArray[i].transform.childCount != 2) { return; }
        }

        for (int i = 0; i < chestArray.Length; i++)     //so that no chest is empty. Now checking whether the options are placed corrected
        {
            if (chestArray[i].transform != optionsArray[mtfValues.correctOrder[i] - 1].transform.parent)
            {
                Reset();
                thisSpacebox.OnMiniGameClosed(false);       //not solved
                return;
            }
        }

        thisSpacebox.OnMiniGameClosed(true);        //Solved
    }

    public void CloseButton()
    {
        Reset();
        thisSpacebox.OnMiniGameClosed(false);       //not solved
    }

    private void Reset()        //resets the gameobject options
    {
        for (int i = 0; i < chestArray.Length; i++)
        {
            optionsArray[i].transform.SetParent(optionsParent.transform);
        }
    }

    [PunRPC]
    private void SetQuestion(int questionNo)
    {
        mtfValues = mtfQuestions[questionNo];
        chestA = chestArray[0].GetComponentInChildren<TextMeshProUGUI>();
        chestB = chestArray[1].GetComponentInChildren<TextMeshProUGUI>();
        chestC = chestArray[2].GetComponentInChildren<TextMeshProUGUI>();
        optionA = optionsArray[0].GetComponentInChildren<TextMeshProUGUI>();
        optionB = optionsArray[1].GetComponentInChildren<TextMeshProUGUI>();
        optionC = optionsArray[2].GetComponentInChildren<TextMeshProUGUI>();

        chestA.text = mtfValues.textA;
        chestB.text = mtfValues.textB;
        chestC.text = mtfValues.textC;
        optionA.text = mtfValues.optionA;
        optionB.text = mtfValues.optionB;
        optionC.text = mtfValues.optionC;
    }



    [PunRPC]
    private void ShowButtonClicked(int buttonNum, int chestNum)
    {
        Debug.Log("RPC is run");
        optionsArray[buttonNum].transform.SetParent(chestArray[chestNum].transform);
        optionsArray[buttonNum].transform.localPosition = new Vector3(0, 250f, 0);
    }
}
