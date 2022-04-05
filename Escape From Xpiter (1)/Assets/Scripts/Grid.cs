using UnityEngine;
using TMPro;
using System.Collections;
using Photon.Pun;
using System.IO;
using System;
using UnityEngine.InputSystem;

public class Grid : MonoBehaviour
{
    //[SerializeField] private GameObject newTile = null;
    [SerializeField] private int gridSize = 0;
    [SerializeField] private TMP_Text moveCounter = null;

    [SerializeField] private GameObject rewardCanvas = null;
    [SerializeField] private GameObject SpaceShipJigsawCanvas;
    bool oxygenTankDeployed;
    bool spaceBoxDeployed;
    int wallsDeployedCount = 0;

    private GameObject newTile;
    private Vector3 tilePosition = Vector3.zero;
    private int moveCount = 0;
    int index1;
    int index2;

    private void OnEnable()
    {
        Spacebox.GiveReward += HandleRewards;
        PlayerController._SpaceshipJigsaw += HandleSpaceshipJigsaw;
    }

    private void OnDisable()
    {
        Spacebox.GiveReward -= HandleRewards;
        PlayerController._SpaceshipJigsaw -= HandleSpaceshipJigsaw;
    }

    private void HandleRewards(bool isSolved)
    {
        if (!isSolved) { return; }
        rewardCanvas.SetActive(true);
        StartCoroutine(ShowRewards());
    }

    private IEnumerator ShowRewards()
    {
        yield return new WaitForSeconds(4f);
        rewardCanvas.SetActive(false);
    }

    public void HandleSpaceshipJigsaw()

    {
        if (!SpaceShipJigsawCanvas.activeInHierarchy)
        {
            SpaceShipJigsawCanvas.SetActive(true);
            if (Cursor.lockState != CursorLockMode.Confined)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

        }
        else
        {
            Debug.Log("Close button clicked");
            GetComponent<PhotonView>().RPC("CloseJigsaw", RpcTarget.All);


        }
    }

    [PunRPC]
    void CloseJigsaw()
    {
        SpaceShipJigsawCanvas.SetActive(false);
        PlayerController.instance.HandleInputChange();
        Cursor.lockState = CursorLockMode.None;
    }
    void GenerateRandomIndexes()
    {
        //Random.InitState(42);
        index1 = UnityEngine.Random.Range(0, 4);
        while (index2 == index1)
        {
            index2 = UnityEngine.Random.Range(0, 4);
        }
    }


}
