using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class RoomListing : MonoBehaviourPunCallbacks
{
    [SerializeField] Text roomDetails;
    public RoomInfo _roomInfo { get; private set; }
    public void SetRoomInfo(RoomInfo roomInfo)
    {
        _roomInfo = roomInfo;
        roomDetails.text = _roomInfo.MaxPlayers + "," + _roomInfo.Name;
    }
    public void OnClickRoomName()
    {
        PhotonNetwork.JoinRoom(_roomInfo.Name);
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Joining a room failed!");

    }

}
