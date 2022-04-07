using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class AssemblyCenter : MonoBehaviour
{
    private int totalButtonsPressed = 0;
    private PhotonView myPV;

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

}
