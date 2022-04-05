using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DelayWaitingRoomController : MonoBehaviourPunCallbacks
{
    PhotonView myPv;
    [SerializeField] private int menuSceneindex;
    [SerializeField] private int multiplayerSceneindex;
    [SerializeField] private int gridNum;

    private int playersJoined;
    private int roomSize;
    [SerializeField] private int minPlayerstoStart;

    [SerializeField] private Text roomCountDisplay;
    [SerializeField] private Text timetostartDisplay;

    private bool readyToCountdown;
    private bool readyToStart;
    private bool startingGame;

    private float timeToStartGame;
    private float fullGameTimer;
    private float notFullGameTimer;

    [SerializeField] private float maxWaitingTime;
    [SerializeField] private float maxFullGameWaitTime;

    // Astronaut Color
    [SerializeField] GameObject astroFace;
    [SerializeField] Color[] colors;


    void Start()
    {

        myPv = GetComponent<PhotonView>();
        fullGameTimer = maxFullGameWaitTime;
        timeToStartGame = maxWaitingTime;
        notFullGameTimer = maxWaitingTime;
        PlayerCountUpdate();

    }

    private void PlayerCountUpdate()
    {
        playersJoined = PhotonNetwork.PlayerList.Length;
        roomSize = PhotonNetwork.CurrentRoom.MaxPlayers;
        roomCountDisplay.text = playersJoined + ":" + roomSize;
        //  Debug.Log(PhotonNetwork.PlayerList[PhotonNetwork.PlayerList.Length - 1].UserId);
        if (playersJoined == roomSize)
        {
            readyToStart = true;
        }
        else if (playersJoined >= minPlayerstoStart)
        {
            readyToCountdown = true;
        }
        else
        {
            readyToCountdown = false;
            readyToStart = false;
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // whenever a new player joins the room
        PlayerCountUpdate();
        // synchronizing countdown timer of masterclient with other clients   
        if (PhotonNetwork.IsMasterClient)
        {
            myPv.RPC("RPC_SendTimer", RpcTarget.Others, timeToStartGame);
        }

    }
    public override void OnCreatedRoom()
    {
        Debug.Log(PhotonNetwork.CurrentRoom.Name + "created!");
    }
    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.CurrentRoom.Name + "joined!");
    }
    [PunRPC]
    private void RPC_SendTimer(float timeIn)
    {
        timeToStartGame = timeIn;
        notFullGameTimer = timeIn;
        if (timeIn < fullGameTimer)
        {
            fullGameTimer = timeIn;
        }

    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PlayerCountUpdate();
    }
    private void Update()
    {
        WaitingForMorePlayers();
    }
    void WaitingForMorePlayers()
    {
        /* if (playersJoined <= 1)
         {
             ResetTimer();
         }*/
        if (readyToStart)
        {
            fullGameTimer -= Time.deltaTime;
            timeToStartGame = fullGameTimer;
        }
        if (readyToCountdown)
        {
            notFullGameTimer -= Time.deltaTime;
            timeToStartGame = notFullGameTimer;
        }
        string tempTimer = string.Format("{0:00}", timeToStartGame);
        timetostartDisplay.text = tempTimer;
        if (timeToStartGame <= 0f)
        {
            if (startingGame)
            {
                return;
            }
            StartGame();
        }
    }
    void ResetTimer()
    {
        timeToStartGame = maxWaitingTime;
        notFullGameTimer = maxWaitingTime;
        fullGameTimer = maxFullGameWaitTime;
    }
    void StartGame()
    {

        startingGame = true;
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        PhotonNetwork.CurrentRoom.IsOpen = false;     // if someone gets disconnected then they will not be able to join again
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.LoadLevel(gridNum);
    }
    public void DelayCancel()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(menuSceneindex);

    }
    public void SetPlayerColor(int index)
    {
        astroFace.GetComponent<Image>().color = colors[index];
        PlayerController.instance.SetAstronautColor(colors[index]);
        Debug.Log("AstronautColor function called");
    }

    public void SetPlayerName(TMP_InputField value)
    {
        PlayerController.instance.SetAstronautName(value.text);
    }

    public void SetGrid(int gridNo)
    {
        gridNum = gridNo;
    }
}
