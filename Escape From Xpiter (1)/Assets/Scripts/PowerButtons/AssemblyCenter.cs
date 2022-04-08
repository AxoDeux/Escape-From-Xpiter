using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(PhotonView))]
public class AssemblyCenter : MonoBehaviour
{
    [SerializeField] private GameObject[] powerButtons = null;      //the one having materials
    [SerializeField] private Texture greenTexture = null;

    public static int totalButtonsPressed = 0;
    [SerializeField] private GameObject InstructionPanel;

    private PhotonView myPV;


    private void OnEnable()
    {
        PlayerController._SpaceshipJigsaw += CloseInstructionPanel;
    }
    private void OnDisable()
    {
        PlayerController._SpaceshipJigsaw -= CloseInstructionPanel;
    }
    private void Awake()
    {
        myPV = GetComponent<PhotonView>();
    }

    public void IncreaseCount(GameObject powerButton)
    {
        int buttonNum = 0;
        if (powerButton == powerButtons[0]) { buttonNum = 0; }
        else { buttonNum = 1; }
        myPV.RPC(nameof(AddButtonsPressed), RpcTarget.All, buttonNum);
    }


    private void CheckCount()
    {
        if (totalButtonsPressed == 2)
        {
            Debug.Log("Two Buttons are pressed");
        }
    }

    [PunRPC]
    private void AddButtonsPressed(int buttonNum)
    {
        totalButtonsPressed++;
        CheckCount();
        powerButtons[buttonNum].GetComponent<Renderer>().material.mainTexture = greenTexture;
        Debug.Log($"Buttons pressed = {totalButtonsPressed}");
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (totalButtonsPressed < 2)
            {
                InstructionPanel.SetActive(true);
                InstructionPanel.transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                InstructionPanel.SetActive(true);
                InstructionPanel.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (totalButtonsPressed < 2)
            {
                InstructionPanel.SetActive(false);
                InstructionPanel.transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                InstructionPanel.SetActive(false);
                InstructionPanel.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
    }
    void CloseInstructionPanel()
    {
        InstructionPanel.SetActive(false);
        InstructionPanel.transform.GetChild(0).gameObject.SetActive(false);
    }
}
