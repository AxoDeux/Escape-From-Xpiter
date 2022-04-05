using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.EventSystems;

public class MCQManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI question = null;
    [SerializeField] private Button[] options = new Button[4];
    [SerializeField] private Spacebox thisSpacebox = null;
    [SerializeField] private AudioSource buttonClick = null;

    [SerializeField] private Sprite optionImg = null;
    [SerializeField] private Sprite optionSelectedImg = null;

    [SerializeField] private MCQScriptableObject[] questions;
    private MCQScriptableObject mcqValues = null;

    private TextMeshProUGUI textA;
    private TextMeshProUGUI textB;
    private TextMeshProUGUI textC;
    private TextMeshProUGUI textD;

    private PhotonView myPhotonView;

    private Button selectedButton = null;

    private void OnEnable()
    {
        thisSpacebox = transform.parent.parent.GetComponent<Spacebox>();
    }

    private void Start()
    {
        myPhotonView = PhotonView.Get(this);

        if (PhotonNetwork.IsMasterClient)                               //Set the question once
        {
            int randomNo = UnityEngine.Random.Range(0, questions.Length);
            myPhotonView.RPC(nameof(SetQuestion), RpcTarget.All, randomNo);
        }
    }

    public void OnOptionClicked(Button clickedButton)
    {
        myPhotonView.RPC(nameof(ShowOptionSelected), RpcTarget.All, clickedButton.name);
        selectedButton = clickedButton;
    }

    public void SubmitOption()
    {
        if (selectedButton == options[mcqValues.rightOption - 1])
        {
            Debug.Log("Correct Option Selected");
            buttonClick.Play();
            //Spacebox Opened sucessfully
            thisSpacebox.OnMiniGameClosed(true);
        }

        else
        {
            thisSpacebox.OnMiniGameClosed(false);
        }
    }

    public void CloseButton()
    {
        thisSpacebox.OnMiniGameClosed(false);
    }

    [PunRPC]
    private void SetQuestion(int questionNo)
    {
        mcqValues = questions[questionNo];

        textA = options[0].GetComponentInChildren<TextMeshProUGUI>();
        textB = options[1].GetComponentInChildren<TextMeshProUGUI>();
        textC = options[2].GetComponentInChildren<TextMeshProUGUI>();
        textD = options[3].GetComponentInChildren<TextMeshProUGUI>();

        textA.text = mcqValues.Option1;
        textB.text = mcqValues.Option2;
        textC.text = mcqValues.Option3;
        textD.text = mcqValues.Option4;
        question.text = mcqValues.Question;
    }

    [PunRPC]
    private void ShowOptionSelected(string optionName)
    {
        for (int i = 0; i < options.Length; i++)
        {
            options[i].image.sprite = optionImg;
        }

        for (int i = 0; i< options.Length; i++)
        {
            if (optionName == options[i].name)
            {
                options[i].image.sprite = optionSelectedImg;
                return;
            }
        }
    }
}
