using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class AssemblyCenter : MonoBehaviour
{
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

    public void IncreaseCount()
    {
        myPV.RPC(nameof(AddButtonsPressed), RpcTarget.All);
    }

    public void DecreaseCount()
    {
        myPV.RPC(nameof(SubtractButtonsPressed), RpcTarget.All);
    }

    private void CheckCount()
    {
        if (totalButtonsPressed == 2)
        {
            Debug.Log("Two Buttons are pressed");
        }
    }

    [PunRPC]
    private void AddButtonsPressed()
    {
        totalButtonsPressed++;
        CheckCount();
        Debug.Log($"Buttons pressed = {totalButtonsPressed}");
    }

    [PunRPC]
    private void SubtractButtonsPressed()
    {
        totalButtonsPressed--;
        CheckCount();
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
        }
    }
    void CloseInstructionPanel()
    {
        InstructionPanel.SetActive(false);
        InstructionPanel.transform.GetChild(0).gameObject.SetActive(false);
    }
}
