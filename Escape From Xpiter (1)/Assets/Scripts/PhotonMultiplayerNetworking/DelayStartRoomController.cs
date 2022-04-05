using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine.UI;

public class DelayStartRoomController : MonoBehaviourPunCallbacks
{
    [SerializeField] private int waitingRoomSceneIndex;
    [SerializeField] private RoomListing _roomListing;
    [SerializeField] private Transform _content;

    List<RoomListing> _listings = new List<RoomListing>();
    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }
    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    public override void OnJoinedRoom()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Room Joined");
            PhotonNetwork.LoadLevel(waitingRoomSceneIndex);
        }
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            // Existing room closed 

            if (info.RemovedFromList)
            {
                int index = _listings.FindIndex(x => x._roomInfo.Name == info.Name);
                if (index != -1)
                {

                    Destroy(_listings[index].gameObject);
                    _listings.RemoveAt(index);
                }
            }
            // Room created
            else
            {

                RoomListing _listing = Instantiate(_roomListing, _content);
                if (_listing != null)
                {
                    _listing.SetRoomInfo(info);
                    _listings.Add(_listing);
                }
            }

        }
    }

}
