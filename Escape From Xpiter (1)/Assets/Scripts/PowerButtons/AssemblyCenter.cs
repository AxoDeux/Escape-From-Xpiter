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
    

    private int totalButtonsPressed = 0;
    private PhotonView myPV;

    private void Awake()
    {
        myPV = GetComponent<PhotonView>();
    }

    public void IncreaseCount(GameObject powerButton)
    {
        int buttonNum = 0;
        if(powerButton == powerButtons[0]) { buttonNum = 0; }
        else { buttonNum = 1; }
        myPV.RPC(nameof(AddButtonsPressed), RpcTarget.All,buttonNum);
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
    private void AddButtonsPressed(int buttonNum)
    {
        totalButtonsPressed++;
        CheckCount();
        powerButtons[buttonNum].GetComponent<Renderer>().material.mainTexture = greenTexture;
        Debug.Log($"Buttons pressed = {totalButtonsPressed}");
    }

    [PunRPC]
    private void SubtractButtonsPressed()
    {
        totalButtonsPressed--;
        CheckCount();
        Debug.Log($"Buttons pressed = {totalButtonsPressed}");
    }

}
