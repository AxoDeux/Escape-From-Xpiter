using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    //[SerializeField] private GameObject[] players = new GameObject[3];
    [SerializeField] private AudioSource buttonClick = null;
    [SerializeField] private TMP_Text energyConsumed;
    [SerializeField] private TMP_Text timeTaken;
    [SerializeField] private TMP_Text spacePartsCollected;
    [SerializeField] private TMP_Text GameFinishedText;



    private void OnEnable()
    {
        energyConsumed.text = PlayerController.totalMoves.ToString();
        timeTaken.text = (900f - UI_Manager._time).ToString("0");
        spacePartsCollected.text = RewardCanvas.count.ToString();

        if (!UI_Manager.isTimeUp)
        {
            GameFinishedText.text = "GAME COMPLETED";
        }
        else
        {
            GameFinishedText.text = "GAME OVER";
        }
    }

    public void LeaveRoom()
    {
        buttonClick.Play();
        StartCoroutine(LeaveRoom_and_Load());

    }
    private IEnumerator LeaveRoom_and_Load()
    {
        Debug.Log("func called");
        PhotonNetwork.LeaveRoom();
        Debug.Log("Leave room called");
        while (PhotonNetwork.InRoom)
            yield return null;
        this.gameObject.SetActive(false);
        Debug.Log("Game over canvas disabled");
        SceneManager.LoadScene(0);
    }
}
