using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class DelayStartLobbyController : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject buttonLoading;
    [SerializeField] private GameObject buttonJoinRoom;
    [SerializeField] private GameObject buttonCancel;
    [SerializeField] private int RoomSize;
    [SerializeField] Text _roomName;

    [SerializeField] private AudioSource buttonClick;

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.JoinLobby();
        buttonLoading.SetActive(false);
        buttonJoinRoom.SetActive(true);
    }
    public void OnClickButtonJoinRoom()
    {

        buttonJoinRoom.SetActive(false);
        buttonCancel.SetActive(true);
        buttonClick.Play();
        Debug.Log("Joining!");
        //PhotonNetwork.JoinRandomRoom();
        CreateRoom();
    }
    public void OnClickButtonCancel()
    {
        buttonCancel.SetActive(false);
        buttonClick.Play();
        //PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        //    buttonJoinRoom.SetActive(true);
    }
    /* public override void OnJoinRandomFailed(short returnCode, string message)
     {
         Debug.Log("Room Joining failed");
         CreateRoom();
     }*/
    public void CreateRoom()
    {
        //int roomCode = Random.Range(1, 10000);
        if (_roomName.text == null)
        {
            Debug.Log("Room Name can't be empty");
            return;
        }
        RoomOptions roomOptions = new RoomOptions() { IsOpen = true, IsVisible = true, MaxPlayers = (byte)RoomSize, PublishUserId = true };
        //PhotonNetwork.JoinOrCreateRoom(_roomName.text, roomOptions, TypedLobby.Default);
        PhotonNetwork.CreateRoom(_roomName.text, roomOptions, TypedLobby.Default);

    }
    /*  public override void OnCreateRoomFailed(short returnCode, string message)
      {
          Debug.Log("Create room failed");
          CreateRoom();
      }*/

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("creating" + _roomName.text + "failed");
        CreateRoom();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        buttonLoading.SetActive(true);
        PhotonNetwork.ConnectUsingSettings();
    }
}
